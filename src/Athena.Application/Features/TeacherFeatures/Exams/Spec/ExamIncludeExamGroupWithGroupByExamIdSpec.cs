namespace Athena.Application.Features.TeacherFeatures.Exams.Spec
{
    public class ExamIncludeExamGroupWithGroupByExamIdSpec : Specification<Exam>, ISingleResultSpecification
    {
        public ExamIncludeExamGroupWithGroupByExamIdSpec(Guid examId) =>
            Query.Where(e => e.Id == examId)
                .Include(e => e.ExamGroups)
                .ThenInclude(e => e.Group);
                
    }
}
