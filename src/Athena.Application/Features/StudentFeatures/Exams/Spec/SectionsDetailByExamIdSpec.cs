namespace Athena.Application.Features.StudentFeatures.Exams.Spec
{
    public class SectionsDetailByExamIdSpec : Specification<Section>
    {
        public SectionsDetailByExamIdSpec(Guid examId) =>
            Query.Where(e => e.ExamId == examId)
                .Include(e => e.SectionImages);
    }
}
