namespace Athena.Application.Features.TeacherFeatures.Exams.Spec
{
    public class ExamIncludeSemseterByExamIdSpec : Specification<Exam>, ISingleResultSpecification
    {
        public ExamIncludeSemseterByExamIdSpec(Guid Id) =>
            Query.Where(e => e.Id == Id)
                .Include(e => e.TeacherCourseLevelYear);
    }
}
