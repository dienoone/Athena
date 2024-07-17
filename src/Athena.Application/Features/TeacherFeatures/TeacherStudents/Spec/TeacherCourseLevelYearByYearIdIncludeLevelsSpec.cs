namespace Athena.Application.Features.TeacherFeatures.TeacherStudents.Spec
{
    public class TeacherCourseLevelYearByYearIdIncludeLevelsSpec : Specification<TeacherCourseLevelYear>
    {
        public TeacherCourseLevelYearByYearIdIncludeLevelsSpec(Guid yearId) =>
            Query.Where(e => e.YearId == yearId)
                .Include(e => e.TeacherCourseLevel.Level)
                .Include(e => e.TeacherCourseLevelYearStudents.Where(e => e.DeletedOn == null))
                .ThenInclude(e => e.Student);
    }
}
