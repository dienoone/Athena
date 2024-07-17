using Athena.Infrastructure.Common;
using Athena.Infrastructure.Persistence.Context;
using Athena.Infrastructure.Persistence.Initialization;
using Athena.Infrastructure.Persistence.Repository;
using Microsoft.Extensions.Options;
using Serilog;

namespace Athena.Infrastructure.Persistence
{
    internal static class Startup
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(Startup));

        internal static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration config)
        {
            services.AddOptions<DatabaseSettings>()
                .BindConfiguration(nameof(DatabaseSettings))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            return services
                .AddDbContext<ApplicationDbContext>((p, m) =>
                {
                    var databaseSettings = p.GetRequiredService<IOptions<DatabaseSettings>>().Value;
                    m.UseSqlServer(databaseSettings.ConnectionString, e => e.MigrationsAssembly("Athena.Migrators"));
                    
                })

                .AddTransient<IDatabaseInitializer, DatabaseInitializer>()
                .AddRepositories();
        }
       
        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            // Add Repositories
            services.AddScoped(typeof(IRepository<>), typeof(ApplicationDbRepository<>));

            foreach (var aggregateRootType in
                typeof(IAggregateRoot).Assembly.GetExportedTypes()
                    .Where(t => typeof(IAggregateRoot).IsAssignableFrom(t) && t.IsClass)
                    .ToList())
            {
                // Add ReadRepositories.
                services.AddScoped(typeof(IReadRepository<>).MakeGenericType(aggregateRootType), sp =>
                    sp.GetRequiredService(typeof(IRepository<>).MakeGenericType(aggregateRootType)));

                // Decorate the repositories with EventAddingRepositoryDecorators and expose them as IRepositoryWithEvents.
                services.AddScoped(typeof(IRepositoryWithEvents<>).MakeGenericType(aggregateRootType), sp =>
                    Activator.CreateInstance(
                        typeof(EventAddingRepositoryDecorator<>).MakeGenericType(aggregateRootType),
                        sp.GetRequiredService(typeof(IRepository<>).MakeGenericType(aggregateRootType)))
                    ?? throw new InvalidOperationException($"Couldn't create EventAddingRepositoryDecorator for aggregateRootType {aggregateRootType.Name}"));
            }

            return services;
        }
    }
}
