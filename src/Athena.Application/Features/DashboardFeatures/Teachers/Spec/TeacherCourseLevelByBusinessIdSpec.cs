namespace Athena.Application.Features.DashboardFeatures.Teachers.Spec
{
    public class TeacherCourseLevelByBusinessIdSpec : Specification<TeacherCourseLevel>
    {
        public TeacherCourseLevelByBusinessIdSpec(Guid busienssId) =>
            Query
            .Where(e => e.BusinessId == busienssId)
            .Include(e => e.Level);

    }
}
