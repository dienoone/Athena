namespace Athena.Application.Features.TeacherFeatures.Exams.Spec
{
    public class ExamGroupStudentIncludeExamByExamGroupStudentIdSpec : Specification<ExamGroupStudent>, ISingleResultSpecification
    {
        public ExamGroupStudentIncludeExamByExamGroupStudentIdSpec(Guid id) =>
            Query.Where(e => e.Id == id)
                .Include(e => e.ExamGroup.Exam)
                .Include(e => e.ExamStudentAnswers);
    }
}
