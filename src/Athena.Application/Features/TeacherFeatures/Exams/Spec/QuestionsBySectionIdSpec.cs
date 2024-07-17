namespace Athena.Application.Features.TeacherFeatures.Exams.Spec
{
    public class QuestionsBySectionIdSpec : Specification<Question>
    {
        public QuestionsBySectionIdSpec(Guid sectionId) =>
            Query.Where(e => e.SectionId == sectionId);
    }
}
