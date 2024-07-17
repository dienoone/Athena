namespace Athena.Application.Features.TeacherFeatures.Exams.Spec
{
    public class ExamGroupStudentsIncludeStudentByExamGroupIdSpec : Specification<ExamGroupStudent>
    {
        public ExamGroupStudentsIncludeStudentByExamGroupIdSpec(Guid examGroupId) =>
            Query.Where(e => e.ExamGroupId == examGroupId)
                .Include(e => e.GroupStudent.TeacherCourseLevelYearStudent.Student)
                .Include(e => e.ExamStudentAnswers);
    }
}
