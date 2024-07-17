namespace Athena.Application.Features.TeacherFeatures.HeadQuarters.Spec
{
    public class HeadQuarterListIncPhonesSpec : Specification<HeadQuarter>
    {
        public HeadQuarterListIncPhonesSpec(Guid businessId) =>
            Query.Where(e => e.BusinessId == businessId)
                .Include(e => e.HeadQuarterPhones.Where(e => e.DeletedOn == null));
    }
}
