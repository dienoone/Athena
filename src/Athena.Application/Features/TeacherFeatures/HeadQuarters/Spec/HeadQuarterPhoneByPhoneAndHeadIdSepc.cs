namespace Athena.Application.Features.TeacherFeatures.HeadQuarters.Spec
{
    public class HeadQuarterPhoneByPhoneAndHeadIdSepc : Specification<HeadQuarterPhone>, ISingleResultSpecification
    {
        public HeadQuarterPhoneByPhoneAndHeadIdSepc(string phone, Guid headId) =>
            Query.Where(e => e.Phone == phone && e.HeadQuarterId == headId);
    }
}
