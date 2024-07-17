using Athena.Application.Common.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Athena.Infrastructure.Auth.Jwt
{
    public class ConfigureJwtBearerOptions : IConfigureNamedOptions<JwtBearerOptions>
    {
        private readonly JwtSettings _jwtSettings;
        private readonly ILanguageService _languageService;

        public ConfigureJwtBearerOptions(IOptions<JwtSettings> jwtSettings, ILanguageService languageService)
        {
            _jwtSettings = jwtSettings.Value;
            _languageService = languageService;
        }

        public void Configure(JwtBearerOptions options)
        {
            Configure(string.Empty, options);
        }

        public void Configure(string name, JwtBearerOptions options)
        {
            if (name != JwtBearerDefaults.AuthenticationScheme)
            {
                return;
            }

            byte[] key = Encoding.ASCII.GetBytes(_jwtSettings.Key);

            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateLifetime = true,
                ValidateAudience = false,
                RoleClaimType = ClaimTypes.Role,
                ClockSkew = TimeSpan.Zero
            };
            options.Events = new JwtBearerEvents
            {
                OnChallenge = context =>
                {
                    context.HandleResponse();
                    if (!context.Response.HasStarted)
                    {
                        throw new UnauthorizedException("Authentication Failed.");
                    }

                    return Task.CompletedTask;
                },
                OnForbidden = _ => throw new ForbiddenException("You are not authorized to access this resource."),
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];
                    
                    if (!string.IsNullOrEmpty(accessToken))   
                    {
                        if (context.HttpContext.Request.Path.StartsWithSegments("/notifications"))
                        {
                            // Read the token out of the query string
                            context.Token = accessToken;

                            // Access the language from the header or any other source
                            var language = context.Request.Headers["accept-language"].ToString();
                            Console.WriteLine($"Language in side hub: {language}");
                            _languageService.SetCurrentLanguage(string.IsNullOrEmpty(language) ? "ar" : language);
                        }
                        else if (context.HttpContext.Request.Path.StartsWithSegments("/takeexam"))
                        {
                            var examId = context.Request.Query["examId"];
                            if (!string.IsNullOrEmpty(examId))
                            {
                                // Read the token out of the query string
                                context.Token = accessToken;
                                
                            }
                        }
                    }

                    return Task.CompletedTask;
                }
            };
        }
    }
}
