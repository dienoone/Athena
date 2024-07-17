using Athena.Infrastructure.Auth;
using Athena.Infrastructure.Common;
using Hangfire.Client;
using Hangfire.Logging;
using Microsoft.AspNetCore.Http;

namespace Athena.Infrastructure.BackgroundJobs
{
    public class AJobFilter : IClientFilter
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        private readonly IServiceProvider _services;

        public AJobFilter(IServiceProvider services)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
        }

        public void OnCreating(CreatingContext context)
        {
            ArgumentNullException.ThrowIfNull(context, nameof(context));

            Logger.InfoFormat("Set BusinessId and UserId parameters to job {0}.{1}...", context.Job.Method.ReflectedType?.FullName, context.Job.Method.Name);

            using var scope = _services.CreateScope();

            var httpContext = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>()?.HttpContext;
            _ = httpContext ?? throw new InvalidOperationException("Can't create a Job without HttpContext.");

            var currentUserInitializer = scope.ServiceProvider.GetRequiredService<ICurrentUserInitializer>();
            currentUserInitializer.SetCurrentUser(httpContext.User);

            var currentUser = scope.ServiceProvider.GetRequiredService<ICurrentUser>();

            var businessId = currentUser.GetBusinessId();
            if (!string.IsNullOrEmpty(businessId.ToString()))
            {
                context.SetJobParameter("BusinessId", businessId);
            }

            string? userId = currentUser.GetUserId().ToString();
            context.SetJobParameter(QueryStringKeys.UserId, userId);
        }

        public void OnCreated(CreatedContext context) =>
            Logger.InfoFormat(
                "Job created with parameters {0}",
                context.Parameters.Select(x => x.Key + "=" + x.Value).Aggregate((s1, s2) => s1 + ";" + s2));
    }

}
