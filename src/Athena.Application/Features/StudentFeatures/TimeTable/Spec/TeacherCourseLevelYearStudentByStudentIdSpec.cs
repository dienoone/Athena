namespace Athena.Application.Features.StudentFeatures.TimeTable.Spec
{
    public class TeacherCourseLevelYearStudentByStudentIdSpec : Specification<TeacherCourseLevelYearStudent>
    {
        public TeacherCourseLevelYearStudentByStudentIdSpec(Guid studentId) =>
            Query.Where(e => e.StudentId == studentId)
                .Include(e => e.TeacherCourseLevelYear.TeacherCourseLevel.Teacher)
                .ThenInclude(e => e.Course);
               
               
    }
}
