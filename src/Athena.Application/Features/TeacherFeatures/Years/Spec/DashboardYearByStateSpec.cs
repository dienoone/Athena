namespace Athena.Application.Features.TeacherFeatures.Years.Spec
{
    public class DashboardYearByStateSpec : Specification<DashboardYear>, ISingleResultSpecification
    {
        public DashboardYearByStateSpec(string state) =>
            Query.Where(e => e.State == state && !e.IsFinished);
    }
}
