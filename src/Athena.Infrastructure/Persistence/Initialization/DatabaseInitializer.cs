using Athena.Infrastructure.Identity;
using Athena.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Athena.Infrastructure.Persistence.Initialization
{
    internal class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly ILogger<DatabaseInitializer> _logger;
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManger;

        public DatabaseInitializer(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManger, 
            ILogger<DatabaseInitializer> logger)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManger = roleManger;
            _logger = logger;

        }
       
        public async Task InitializeDatabasesAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Welocme To My World!");
            await IdentityContextSeed.SeedDatabaseAsync(_roleManger, _userManager, _dbContext, cancellationToken);
            await ApplicationContextSeed.CoursesSeed(_dbContext, cancellationToken);
            await ApplicationContextSeed.LevelClassificationSeed(_dbContext, cancellationToken);
        }
    }
}
