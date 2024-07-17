namespace Athena.Application.Features.TeacherFeatures.Years.Spec
{
    public class YearByStateAndBusinessIdSpec : Specification<Year>, ISingleResultSpecification
    {
        public YearByStateAndBusinessIdSpec(string state, Guid businessId) =>
            Query.Where(e => e.YearState == state && e.BusinessId == businessId && e.State);
    }
}
