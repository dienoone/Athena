using Ardalis.Specification;

namespace Athena.Infrastructure.Notifications.Spec
{
    public class GetSignalRConnectionByConnectionIdSpec : Specification<SignalRConnection>, ISingleResultSpecification
    {
        public GetSignalRConnectionByConnectionIdSpec(string connectionId) =>
            Query.Where(e => e.ConnectionId == connectionId);
    }
}
