using Ardalis.Specification;

namespace Athena.Infrastructure.Notifications.Spec
{
    public class GetSignalRConnectionByUserIdAndTypeSpec : Specification<SignalRConnection>, ISingleResultSpecification
    {
        public GetSignalRConnectionByUserIdAndTypeSpec(Guid userId, string type) =>
            Query.Where(e => e.UserId == userId && e.Type == type);
    }
}
