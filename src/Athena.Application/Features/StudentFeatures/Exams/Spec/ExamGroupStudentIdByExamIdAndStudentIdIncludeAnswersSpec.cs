namespace Athena.Application.Features.StudentFeatures.Exams.Spec
{
    public class ExamGroupStudentIdByExamIdAndStudentIdIncludeAnswersSpec : Specification<ExamGroupStudent>, ISingleResultSpecification
    {
        public ExamGroupStudentIdByExamIdAndStudentIdIncludeAnswersSpec(Guid examId, Guid studentId) =>
            Query.Where(e => e.ExamGroup.ExamId == examId && e.GroupStudent.TeacherCourseLevelYearStudent.StudentId == studentId)
                .Include(e => e.ExamStudentAnswers.Where(e => e.DeletedOn == null));
    }
}
