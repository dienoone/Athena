namespace Athena.Application.Features.DashboardFeatures.Classification.Spec
{
    public class ClassificationByNameSpec : Specification<EducationClassification>, ISingleResultSpecification
    {
        public ClassificationByNameSpec(string name) =>
            Query.Where(e => e.Name == name);
    }
}
