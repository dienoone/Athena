using Athena.Api.Configurations;
using Athena.Application;
using Athena.Infrastructure;
using Athena.Infrastructure.Common;
using Athena.Infrastructure.Middleware;
using FluentValidation.AspNetCore;
using Serilog;

/*[assembly: ApiConventionType(typeof(AthenaApiConventions))]*/

StaticLogger.EnsureInitialized();
Log.Information("Server Booting Up...");
Log.Information("Marwanitto...");
try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.AddConfigurations();
    builder.Host.UseSerilog((_, config) =>
    {
        config.WriteTo.Console()
        .ReadFrom.Configuration(builder.Configuration);
    });

    builder.Services.AddControllers().AddFluentValidation();
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddApplication();

    var app = builder.Build();

    await app.Services.InitializeDatabasesAsync();

    app.UseInfrastructure(builder.Configuration);
    app.UseHsts();
    app.UseHttpsRedirection();
    app.MapEndpoints();
    app.Run();
}
catch (Exception ex) when (!ex.GetType().Name.Equals("StopTheHostException", StringComparison.Ordinal))
{
    StaticLogger.EnsureInitialized();
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    StaticLogger.EnsureInitialized();
    Log.Information("Server Shutting down...");
    Log.CloseAndFlush();
}
