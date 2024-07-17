namespace Athena.Application.Features.TeacherFeatures.HeadQuarters.Spec
{
    public class HeadQuarterPhoneByPhoneAndBusinessIdSepc : Specification<HeadQuarterPhone>, ISingleResultSpecification
    {
        public HeadQuarterPhoneByPhoneAndBusinessIdSepc(string phone, Guid businessId) =>
           Query.Where(e => e.Phone == phone && e.BusinessId == businessId);
    }
}
