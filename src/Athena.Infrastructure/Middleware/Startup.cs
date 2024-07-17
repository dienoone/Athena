using Athena.Infrastructure.Localization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Athena.Infrastructure.Middleware
{
    internal static class Startup
    {
        internal static IServiceCollection AddExceptionMiddleware(this IServiceCollection services) =>
            services.AddScoped<ExceptionMiddleware>();

        internal static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app) =>
            app.UseMiddleware<ExceptionMiddleware>();

        internal static IServiceCollection AddRequestLogging(this IServiceCollection services, IConfiguration config)
        {
            if (GetMiddlewareSettings(config).EnableHttpsLogging)
            {
                services.AddSingleton<RequestLoggingMiddleware>();
                services.AddScoped<ResponseLoggingMiddleware>();
            }

            return services;
        }

        internal static IServiceCollection AddLanguage(this IServiceCollection services)
        {
            services.AddSingleton<ILanguageService, LanguageService>();
            services.AddTransient<LanguageMiddleware>();
            return services;
        }

        internal static IApplicationBuilder UseLanguage(this IApplicationBuilder app) =>
            app.UseMiddleware<LanguageMiddleware>();

        internal static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app, IConfiguration config)
        {
            if (GetMiddlewareSettings(config).EnableHttpsLogging)
            {
                app.UseMiddleware<RequestLoggingMiddleware>();
                app.UseMiddleware<ResponseLoggingMiddleware>();
            }

            return app;
        }

        private static MiddlewareSettings GetMiddlewareSettings(IConfiguration config) =>
            config.GetSection(nameof(MiddlewareSettings)).Get<MiddlewareSettings>();
    }
}
