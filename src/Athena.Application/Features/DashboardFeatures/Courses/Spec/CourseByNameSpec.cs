namespace Athena.Application.Features.DashboardFeatures.Courses.Spec
{
    public class CourseByNameSpec : Specification<Course>, ISingleResultSpecification
    {
        public CourseByNameSpec(string name) =>
            Query.Where(e => e.Name == name);

    }
}
