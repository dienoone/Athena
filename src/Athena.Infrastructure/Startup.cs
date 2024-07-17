using Athena.Infrastructure.Auth;
using Athena.Infrastructure.BackgroundJobs;
using Athena.Infrastructure.Caching;
using Athena.Infrastructure.Common;
using Athena.Infrastructure.Cors;
using Athena.Infrastructure.FileStorage;
using Athena.Infrastructure.Localization;
using Athena.Infrastructure.Mailing;
using Athena.Infrastructure.Mapping;
using Athena.Infrastructure.Middleware;
using Athena.Infrastructure.Notifications;
using Athena.Infrastructure.OpenApi;
using Athena.Infrastructure.Persistence;
using Athena.Infrastructure.Persistence.Initialization;
using Athena.Infrastructure.SecurityHeaders;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Infrastructure.Test")]

namespace Athena.Infrastructure
{
    public static class Startup
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            MapsterSettings.Configure();
            return services
                .AddApiVersioning()
                .AddAuth(config)
                .AddBackgroundJobs(config)
                .AddCaching(config)
                .AddCorsPolicy(config)
                .AddExceptionMiddleware()
                /*.AddHealthCheck()*/
                .AddPOLocalization(config)
                .AddMailing(config)
                .AddMediatR(Assembly.GetExecutingAssembly())
                .AddNotifications(config)
                .AddOpenApiDocumentation(config)
                .AddPersistence(config)
                .AddRequestLogging(config)
                .AddRouting(options => options.LowercaseUrls = true)
                .AddLanguage()
                .AddServices();
        }

        private static IServiceCollection AddApiVersioning(this IServiceCollection services) =>
            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            });

        /* private static IServiceCollection AddHealthCheck(this IServiceCollection services) =>
             services.AddHealthChecks().AddCheck<TenantHealthCheck>("Tenant").Services;*/

        public static async Task InitializeDatabasesAsync(this IServiceProvider services, CancellationToken cancellationToken = default)
        {
            // Create a new scope to retrieve scoped services
            using var scope = services.CreateScope();

            await scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>()
                .InitializeDatabasesAsync(cancellationToken);
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder builder, IConfiguration config) =>
            builder
                .UseRequestLocalization()
                .UseStaticFiles()
                .UseSecurityHeaders(config)
                .UseFileStorage()
                .UseExceptionMiddleware()
                .UseRouting()
                .UseCorsPolicy()
                .UseAuthentication()
                .UseCurrentUser()
                .UseAuthorization()
                .UseRequestLogging(config)
                .UseHangfireDashboard(config)
                .UseOpenApiDocumentation(config)
                .UseLanguage();

        public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder builder)
        {
            builder.MapControllers().RequireAuthorization();
            /*builder.MapHealthCheck();*/
            builder.MapNotifications();
            return builder;
        }

        /*private static IEndpointConventionBuilder MapHealthCheck(this IEndpointRouteBuilder endpoints) =>
            endpoints.MapHealthChecks("/api/health").RequireAuthorization();*/
    }
}
