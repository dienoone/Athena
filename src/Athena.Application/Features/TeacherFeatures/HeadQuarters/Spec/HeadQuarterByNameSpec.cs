namespace Athena.Application.Features.TeacherFeatures.HeadQuarters.Spec
{
    public class HeadQuarterByNameSpec : Specification<HeadQuarter>, ISingleResultSpecification
    {
        public HeadQuarterByNameSpec(string name, Guid businessId) =>
            Query.Where(e => e.Name == name && e.BusinessId == businessId);

    }
}
