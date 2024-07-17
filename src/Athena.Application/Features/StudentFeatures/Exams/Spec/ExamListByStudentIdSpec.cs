namespace Athena.Application.Features.StudentFeatures.Exams.Spec
{
    public class ExamListByStudentIdSpec : Specification<Exam>
    {
        public ExamListByStudentIdSpec(Guid studentId) =>
            Query.Where(e => e.ExamGroups.Any(eg => 
                eg.ExamGroupStudents.Any(egs => 
                        egs.GroupStudent.TeacherCourseLevelYearStudent.StudentId == studentId)))
                .Include(e => e.TeacherCourseLevelYear.TeacherCourseLevel.Teacher).ThenInclude(e => e.Course);
    }
}
