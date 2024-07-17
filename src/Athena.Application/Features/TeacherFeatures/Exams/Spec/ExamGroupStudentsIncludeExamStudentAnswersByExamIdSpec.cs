namespace Athena.Application.Features.TeacherFeatures.Exams.Spec
{
    public class ExamGroupStudentsIncludeExamStudentAnswersByExamIdSpec : Specification<ExamGroupStudent>
    {
        public ExamGroupStudentsIncludeExamStudentAnswersByExamIdSpec(Guid examId) =>
            Query.Where(e => e.ExamGroup.ExamId == examId)
                .Include(e => e.ExamStudentAnswers)
                .ThenInclude(e => e.Question);
    }
}
