namespace Athena.Application.Features.StudentFeatures.Exams.Spec
{
    public class ExamGroupStudentByStudentIdAndExamIdSpec : Specification<ExamGroupStudent>, ISingleResultSpecification
    {
        public ExamGroupStudentByStudentIdAndExamIdSpec(Guid studentId, Guid examId) =>
            Query.Where(e => e.ExamGroup.ExamId == examId && e.GroupStudent.TeacherCourseLevelYearStudent.StudentId == studentId);
    }
}
