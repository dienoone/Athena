namespace Athena.Application.Features.TeacherFeatures.Years.Spec
{
    public class YearByStartAndBusinessIdSpec : Specification<Year>, ISingleResultSpecification
    {
        public YearByStartAndBusinessIdSpec(int start, Guid businessId) =>
            Query
                .Where(e => e.BusinessId == businessId && e.DashboardYear.Start == start);
    }
}
