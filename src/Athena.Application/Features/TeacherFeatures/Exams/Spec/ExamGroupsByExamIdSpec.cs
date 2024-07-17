namespace Athena.Application.Features.TeacherFeatures.Exams.Spec
{
    public class ExamGroupsByExamIdSpec : Specification<ExamGroup>
    {
        public ExamGroupsByExamIdSpec(Guid ExamId) =>
            Query.Where(e => e.ExamId == ExamId);
    }
}
