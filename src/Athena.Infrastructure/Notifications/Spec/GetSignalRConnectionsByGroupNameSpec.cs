using Ardalis.Specification;

namespace Athena.Infrastructure.Notifications.Spec
{
    public class GetSignalRConnectionsByGroupNameSpec : Specification<SignalRConnection>
    {
        public GetSignalRConnectionsByGroupNameSpec(string groupName) =>
            Query.Where(e => e.Groups.Any(g => g.Name == groupName));
    }
}
