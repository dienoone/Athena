namespace Athena.Application.Features.StudentFeatures.Exams.Spec
{
    public class ExamGroupStudentIdByExamIdAndStudentIdSpec : Specification<ExamGroupStudent>, ISingleResultSpecification
    {
        public ExamGroupStudentIdByExamIdAndStudentIdSpec(Guid examId, Guid studentId) =>
            Query.Where(e => e.ExamGroup.ExamId == examId && e.GroupStudent.TeacherCourseLevelYearStudent.StudentId == studentId);

    }
}
