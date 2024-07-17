namespace Athena.Application.Features.TeacherFeatures.HeadQuarters.Spec
{
    public class HeadQuarterRequiredByBusinessIdSpec : Specification<HeadQuarter>
    {
        public HeadQuarterRequiredByBusinessIdSpec(Guid businessId) =>
            Query.Where(e => e.BusinessId == businessId);
    }
}
