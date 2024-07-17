namespace Athena.Application.Features.StudentFeatures.Exams.Spec
{
    public class QuestionsDetailBySectionIdSpec : Specification<Question>
    {
        public QuestionsDetailBySectionIdSpec(Guid sectionId) =>
            Query.Where(e => e.SectionId == sectionId)
                .Include(e => e.QuestionImages)
                .Include(e => e.QuestionChoices);
    }
}
