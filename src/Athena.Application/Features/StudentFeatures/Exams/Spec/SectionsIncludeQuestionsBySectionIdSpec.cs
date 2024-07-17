namespace Athena.Application.Features.StudentFeatures.Exams.Spec
{
    public class SectionsIncludeQuestionsBySectionIdSpec : Specification<Section>, ISingleResultSpecification
    {
        public SectionsIncludeQuestionsBySectionIdSpec(Guid id) =>
            Query.Where(e => e.Id == id)
                .Include(e => e.Questions);
    }
}
