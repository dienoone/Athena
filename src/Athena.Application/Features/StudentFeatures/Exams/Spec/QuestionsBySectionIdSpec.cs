namespace Athena.Application.Features.StudentFeatures.Exams.Spec
{
    public class QuestionsBySectionIdSpec : Specification<Question>
    {
        public QuestionsBySectionIdSpec(Guid sectionId) =>
            Query.Where(e => e.SectionId == sectionId);
    }
}
