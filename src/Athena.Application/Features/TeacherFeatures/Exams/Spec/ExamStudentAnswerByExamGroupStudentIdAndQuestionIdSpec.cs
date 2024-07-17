namespace Athena.Application.Features.TeacherFeatures.Exams.Spec
{
    public class ExamStudentAnswerByExamGroupStudentIdAndQuestionIdSpec : Specification<ExamStudentAnswer>, ISingleResultSpecification
    {
        public ExamStudentAnswerByExamGroupStudentIdAndQuestionIdSpec(Guid examGroupStudentId, Guid questionId) =>
            Query.Where(e => e.ExamGroupStudentId == examGroupStudentId && e.QuestionId == questionId);
    }
}
