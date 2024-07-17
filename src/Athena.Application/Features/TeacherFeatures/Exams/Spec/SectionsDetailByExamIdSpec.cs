namespace Athena.Application.Features.TeacherFeatures.Exams.Spec
{
    public class SectionsDetailByExamIdSpec : Specification<Section>
    {
        public SectionsDetailByExamIdSpec(Guid examId) =>
            Query.Where(e => e.ExamId == examId)
                .Include(e => e.SectionImages);
    }
}
