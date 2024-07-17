namespace Athena.Application.Features.TeacherFeatures.Groups.Spec
{
    public class GetSignalRConnectionIdsByUserIdsSpec : Specification<SignalRConnection>
    {
        public GetSignalRConnectionIdsByUserIdsSpec(IEnumerable<Guid> userIds) =>
            Query.Where(e => userIds.Contains(e.UserId));
    }
}
