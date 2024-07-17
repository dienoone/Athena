namespace Athena.Application.Features.TeacherFeatures.Exams.Spec
{
    public class ExamGroupsByExamIdIncludeGroupSpec : Specification<ExamGroup>
    {
        public ExamGroupsByExamIdIncludeGroupSpec(Guid examId) =>
            Query.Where(e => e.ExamId == examId)
                .Include(e => e.Group);
    }
}
