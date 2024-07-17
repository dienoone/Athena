namespace Athena.Application.Features.StudentFeatures.Exams.Spec
{
    public class ExamStudentAnswerByQuestionIdAndExamGroupStudnetIdSpec : Specification<ExamStudentAnswer>, ISingleResultSpecification
    {
        public ExamStudentAnswerByQuestionIdAndExamGroupStudnetIdSpec(Guid questionId, Guid examGroupStudentId) =>
            Query.Where(e => e.QuestionId == questionId && e.ExamGroupStudentId == examGroupStudentId);
    }
}
