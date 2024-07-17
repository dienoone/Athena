namespace Athena.Application.Features.TeacherFeatures.Exams.Spec
{
    public class ExamGroupsIncludeGroupByExamIdSpec : Specification<ExamGroup>
    {
        public ExamGroupsIncludeGroupByExamIdSpec(Guid Id) =>
            Query.Where(e => e.ExamId == Id)
                .Include(e => e.Group);
    }
}
