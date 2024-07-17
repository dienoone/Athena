namespace Athena.Application.Features.TeacherFeatures.Exams.Spec
{
    public class ExamStudentAnswersByExamIdSpec : Specification<ExamStudentAnswer>
    {
        public ExamStudentAnswersByExamIdSpec(Guid examId) =>
            Query.Where(e => e.ExamGroupStudent.ExamGroup.ExamId == examId);
    }
}
