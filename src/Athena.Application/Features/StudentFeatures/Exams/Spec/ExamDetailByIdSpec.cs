namespace Athena.Application.Features.StudentFeatures.Exams.Spec
{
    public class ExamDetailByIdSpec : Specification<Exam>, ISingleResultSpecification
    {
        public ExamDetailByIdSpec(Guid Id)
        {
            Query.Where(e => e.Id == Id)
                .Include(e => e.ExamType);
        }

    }
}
