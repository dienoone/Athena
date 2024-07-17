namespace Athena.Application.Features.TeacherFeatures.Exams.Spec
{
    public class ExamResultIncludeExamTypeAndGroupByExamIdSpec : Specification<Exam>, ISingleResultSpecification
    {
        public ExamResultIncludeExamTypeAndGroupByExamIdSpec(Guid Id) =>
            Query.Where(e => e.Id == Id)
                .Include(e => e.ExamGroups.Where(e => e.DeletedOn == null)).ThenInclude(e => e.Group)
                .Include(e => e.ExamType);
    }
}
