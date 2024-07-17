namespace Athena.Application.Features.TeacherFeatures.Exams.Spec
{
    public class SectionsIncludeQuestionsByExamIdSpec : Specification<Section>
    {
        public SectionsIncludeQuestionsByExamIdSpec(Guid examId) =>
            Query.Where(e => e.ExamId == examId)
                .Include(e => e.Questions);
    }
}
