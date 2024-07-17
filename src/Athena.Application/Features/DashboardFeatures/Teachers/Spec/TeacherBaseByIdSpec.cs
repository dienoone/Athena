namespace Athena.Application.Features.DashboardFeatures.Teachers.Spec
{
    public class TeacherBaseByIdSpec : Specification<Teacher>, ISingleResultSpecification
    {
        public TeacherBaseByIdSpec(Guid id) =>
            Query.Where(e => e.Id == id)
            .Include(e => e.Course)
            .IgnoreQueryFilters();

    }
}
