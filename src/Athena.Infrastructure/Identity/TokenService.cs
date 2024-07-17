using Athena.Application.Common.Exceptions;
using Athena.Application.Identity.Tokens;
using Athena.Infrastructure.Auth;
using Athena.Infrastructure.Auth.Jwt;
using Athena.Shared.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Athena.Infrastructure.Identity
{
    internal record TokenHelper(string Token, DateTime ValidTo);
    internal class TokenService : ITokenService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IStringLocalizer _t;
        private readonly SecuritySettings _securitySettings;
        private readonly JwtSettings _jwtSettings;
        

        public TokenService(
            UserManager<ApplicationUser> userManager,
            IOptions<JwtSettings> jwtSettings,
            IStringLocalizer<TokenService> localizer,
            IOptions<SecuritySettings> securitySettings)
        {
            _userManager = userManager;
            _t = localizer;
            _jwtSettings = jwtSettings.Value;
            _securitySettings = securitySettings.Value;
        }

        public async Task<TokenResponse> GetTokenAsync(TokenRequest request, string role, string ipAddress, CancellationToken cancellationToken)
        {

            var user = await _userManager.Users.Where(e => e.Email == request.Email || e.PhoneNumber == request.Email || e.UserName == request.Email).FirstOrDefaultAsync(cancellationToken);
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                throw new UnauthorizedException(_t["Authentication Failed."]);
            }

            if (!user.IsActive)
            {
                throw new UnauthorizedException(_t["User Not Active. Please contact the administrator."]);
            }

            /*if (_securitySettings.RequireConfirmedAccount && !user.EmailConfirmed)
            {
                throw new UnauthorizedException(_t["E-Mail not confirmed."]);
            }*/

            return await GenerateTokensAndUpdateUser(user, role, ipAddress);
        }
        

        public async Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request, string role, string ipAddress)
        {
            var userPrincipal = GetPrincipalFromExpiredToken(request.Token);
            string? userEmail = userPrincipal.GetEmail();
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user is null)
            {
                throw new UnauthorizedException(_t["Authentication Failed."]);
            }

            if (user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                throw new UnauthorizedException(_t["Invalid Refresh Token."]);
            }

            return await GenerateTokensAndUpdateUser(user, role, ipAddress);
        }

        private async Task<TokenResponse> GenerateTokensAndUpdateUser(ApplicationUser user, string role, string ipAddress)
        {
            TokenHelper token = GenerateJwt(user, ipAddress);

            var roles = await _userManager.GetRolesAsync(user);

            if (!roles.Contains(role))
                throw new UnauthorizedException(_t["Authentication Failed."]);

            user.RefreshToken = GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays);

            await _userManager.UpdateAsync(user);

            return new TokenResponse(
                user.UserName,
                user.Email,
                roles,
                token.Token,
                token.ValidTo,
                user.RefreshToken,
                user.RefreshTokenExpiryTime
            );
        }

        private TokenHelper GenerateJwt(ApplicationUser user, string ipAddress) =>
            GenerateEncryptedToken(GetSigningCredentials(), GetClaims(user, ipAddress));

        private static IEnumerable<Claim> GetClaims(ApplicationUser user, string ipAddress) =>
            new List<Claim>
            {
                new(AClaims.UserId, user.Id),
                new(AClaims.BusinessId, user.BusinessId.ToString()),
                new(ClaimTypes.Email, user.Email),
                new(AClaims.Fullname, $"{user.FirstName} {user.LastName}"),
                new(ClaimTypes.Name, user.FirstName ?? string.Empty),
                new(ClaimTypes.Surname, user.LastName ?? string.Empty),
                new(AClaims.IpAddress, ipAddress),
                new(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty)
            };

        private string GenerateRefreshToken()
        {
            byte[] randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private TokenHelper GenerateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
        {
            var token = new JwtSecurityToken(
               claims: claims,
               expires: DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpirationInMinutes),
               signingCredentials: signingCredentials);
            var tokenHandler = new JwtSecurityTokenHandler();
            return new(tokenHandler.WriteToken(token), token.ValidTo);
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RoleClaimType = ClaimTypes.Role,
                ClockSkew = TimeSpan.Zero,
                ValidateLifetime = false
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(
                    SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
            {
                throw new UnauthorizedException(_t["Invalid Token."]);
            }

            return principal;
        }

        private SigningCredentials GetSigningCredentials()
        {
            byte[] secret = Encoding.UTF8.GetBytes(_jwtSettings.Key);
            return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
        }
    }

    
}
