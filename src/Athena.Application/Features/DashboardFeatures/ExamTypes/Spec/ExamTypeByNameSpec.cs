namespace Athena.Application.Features.DashboardFeatures.ExamTypes.Spec
{
    public class ExamTypeByNameSpec : Specification<ExamType>, ISingleResultSpecification
    {
        public ExamTypeByNameSpec(string name) =>
            Query.Where(e => e.Name == name);
    }

}
