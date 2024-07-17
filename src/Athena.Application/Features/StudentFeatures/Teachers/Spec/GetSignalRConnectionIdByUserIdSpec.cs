namespace Athena.Application.Features.StudentFeatures.Teachers.Spec
{
    public class GetSignalRConnectionIdByUserIdSpec : Specification<SignalRConnection>, ISingleResultSpecification
    {
        public GetSignalRConnectionIdByUserIdSpec(Guid userId) =>
            Query.Where(e => e.Id == userId);
    }
}
