namespace Athena.Application.Features.TeacherFeatures.Exams.Spec
{
    public class QuestionDetailsByQuestionIdSpec : Specification<Question>, ISingleResultSpecification
    {
        public QuestionDetailsByQuestionIdSpec(Guid sectionId) =>
           Query.Where(e => e.SectionId == sectionId)
               .Include(e => e.QuestionImages)
               .Include(e => e.QuestionChoices);
    }
}
