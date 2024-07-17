using Ardalis.Specification;

namespace Athena.Infrastructure.Notifications.Spec
{
    public class SignalRConnectionsByUserIdSpec : Specification<SignalRConnection>
    {
        public SignalRConnectionsByUserIdSpec(Guid userId) =>
            Query.Where(e => e.UserId == userId);
    }
}
