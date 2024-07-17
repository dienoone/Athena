namespace Athena.Application.Features.StudentFeatures.Common.Spec
{
    public class TeacherCourseLevelStudentListByStudentIdSpec : Specification<TeacherCourseLevelYearStudent>
    {
        public TeacherCourseLevelStudentListByStudentIdSpec(Guid studentId) =>
            Query.Where(e => e.StudentId == studentId)
            .Include(e => e.TeacherCourseLevelYear.TeacherCourseLevel.Teacher).ThenInclude(e => e.Course);
    }
}
