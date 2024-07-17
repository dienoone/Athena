using Athena.Infrastructure.Auth;
using Hangfire;
using Hangfire.Server;

namespace Athena.Infrastructure.BackgroundJobs
{
    public class AJobActivator : JobActivator
    {
        private readonly IServiceScopeFactory _scopeFactory;
        
        public AJobActivator(IServiceScopeFactory scopeFactory) 
        {
            _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
        }
            

        public override JobActivatorScope BeginScope(PerformContext context) =>
            new Scope(context, _scopeFactory.CreateScope());

        private class Scope : JobActivatorScope, IServiceProvider
        {
            private readonly PerformContext _context;
            private readonly IServiceScope _scope;

            public Scope(PerformContext context, IServiceScope scope)
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
                _scope = scope ?? throw new ArgumentNullException(nameof(scope));

                ReceiveParameters();
            }

            private void ReceiveParameters()
            {
                var businessId = _context.GetJobParameter<string>("BusinessId");
                if (!string.IsNullOrEmpty(businessId))
                {
                    var currentUser = _scope.ServiceProvider.GetRequiredService<ICurrentUser>();
                    currentUser.SetBusinessId(businessId); // Assuming you add this method to your ICurrentUser interface
                }

                string userId = _context.GetJobParameter<string>("UserId");
                if (!string.IsNullOrEmpty(userId))
                {
                    var currentUserInitializer = _scope.ServiceProvider.GetRequiredService<ICurrentUserInitializer>();
                    currentUserInitializer.SetCurrentUserId(userId); // Assuming you add this method to your ICurrentUser interface
                }
            }

            public override object Resolve(Type type) =>
                ActivatorUtilities.GetServiceOrCreateInstance(this, type);

            object? IServiceProvider.GetService(Type serviceType) =>
                serviceType == typeof(PerformContext)
                    ? _context
                    : _scope.ServiceProvider.GetService(serviceType);
        }
    }
}
