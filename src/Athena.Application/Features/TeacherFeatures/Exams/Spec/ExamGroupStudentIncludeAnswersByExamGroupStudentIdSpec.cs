namespace Athena.Application.Features.TeacherFeatures.Exams.Spec
{
    public class ExamGroupStudentIncludeAnswersByExamGroupStudentIdSpec : Specification<ExamGroupStudent>, ISingleResultSpecification
    {
        public ExamGroupStudentIncludeAnswersByExamGroupStudentIdSpec(Guid Id) =>
            Query.Where(e => e.Id == Id)
                .Include(e => e.ExamStudentAnswers.Where(e => e.DeletedOn == null))
                .Include(e => e.GroupStudent.TeacherCourseLevelYearStudent.Student)
                .Include(e => e.ExamGroup);
               
    }
}
