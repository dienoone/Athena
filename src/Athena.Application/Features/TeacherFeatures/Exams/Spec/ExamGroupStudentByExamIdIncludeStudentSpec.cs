namespace Athena.Application.Features.TeacherFeatures.Exams.Spec
{
    public class ExamGroupStudentByExamIdIncludeStudentSpec : Specification<ExamGroupStudent>
    {
        public ExamGroupStudentByExamIdIncludeStudentSpec(Guid examId) =>
            Query.Where(e => e.ExamGroup.ExamId == examId)
                .Include(e => e.GroupStudent.TeacherCourseLevelYearStudent);
    }
}
