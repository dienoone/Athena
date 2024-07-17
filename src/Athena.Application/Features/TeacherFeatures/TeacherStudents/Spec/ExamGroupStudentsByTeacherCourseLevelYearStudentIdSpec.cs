namespace Athena.Application.Features.TeacherFeatures.TeacherStudents.Spec
{
    public class ExamGroupStudentsByTeacherCourseLevelYearStudentIdSpec : Specification<ExamGroupStudent>
    {
        public ExamGroupStudentsByTeacherCourseLevelYearStudentIdSpec(Guid teacherCourseLevelYearStudentId) =>
            Query.Where(e => e.GroupStudent.TeacherCourseLevelYearStudentId == teacherCourseLevelYearStudentId)
                .Include(e => e.ExamGroup)
                .ThenInclude(eg => eg.Exam);

    }
}
