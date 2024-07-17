namespace Athena.Application.Features.TeacherFeatures.TeacherStudents.Spec
{
    public class ExamGroupStudentAnswersByExamGroupStudentIdSpec : Specification<ExamStudentAnswer>
    {
        public ExamGroupStudentAnswersByExamGroupStudentIdSpec(Guid examGroupStudentId) =>
            Query.Where(e => e.ExamGroupStudentId == examGroupStudentId)
                .Include(e => e.Question)
                .ThenInclude(e => e.QuestionChoices);
    }
}
