namespace Athena.Application.Features.TeacherFeatures.Exams.Spec
{
    public class QuestionsByExamIdSpec : Specification<Question>
    {
        public QuestionsByExamIdSpec(Guid examId) =>
            Query.Where(e => e.Section.ExamId == examId);
    }
}
