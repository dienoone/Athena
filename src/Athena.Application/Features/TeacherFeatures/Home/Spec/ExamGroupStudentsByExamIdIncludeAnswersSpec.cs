namespace Athena.Application.Features.TeacherFeatures.Home.Spec
{
    public class ExamGroupStudentsByExamIdIncludeAnswersSpec : Specification<ExamGroupStudent>
    {
        public ExamGroupStudentsByExamIdIncludeAnswersSpec(IEnumerable<Guid> examIds) =>
            Query.Where(e =>  examIds.Contains(e.ExamGroup.ExamId));
    }
}
