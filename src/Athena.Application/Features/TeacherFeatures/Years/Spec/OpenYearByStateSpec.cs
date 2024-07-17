namespace Athena.Application.Features.TeacherFeatures.Years.Spec
{
    public class OpenYearByStateSpec : Specification<Year>, ISingleResultSpecification
    {
        public OpenYearByStateSpec(string state, Guid businessId) =>
            Query.Where(e => e.YearState == state && e.State && e.BusinessId == businessId);
    }
}
