namespace Athena.Application.Features.DashboardFeatures.Years.Spec
{
    public class DashboardYearByStartAndStateSpec : Specification<DashboardYear>, ISingleResultSpecification
    {
        public DashboardYearByStartAndStateSpec(int start, string state) =>
            Query.Where(e => e.Start == start && e.State == state);
    }
}
