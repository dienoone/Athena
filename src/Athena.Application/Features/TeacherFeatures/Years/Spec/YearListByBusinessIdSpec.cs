namespace Athena.Application.Features.TeacherFeatures.Years.Spec
{
    public class YearListByBusinessIdSpec : Specification<Year>
    {
        public YearListByBusinessIdSpec(Guid businessId) =>
            Query.Where(e => e.BusinessId == businessId)
                .Include(e => e.DashboardYear);

    }
}
