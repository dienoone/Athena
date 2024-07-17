namespace Athena.Application.Features.TeacherFeatures.TeacherStudents.Spec
{
    public class OpenTeacherCourseLevelYearsByBusinessIdIncludeLevelAndDashboardYearSpec : Specification<TeacherCourseLevelYear>
    {
        public OpenTeacherCourseLevelYearsByBusinessIdIncludeLevelAndDashboardYearSpec(Guid businessId) =>
            Query.Where(e => e.BusinessId == businessId && e.Year.State)
                .Include(e => e.Year.DashboardYear)
                .Include(e => e.TeacherCourseLevel.Level);
    }
}
