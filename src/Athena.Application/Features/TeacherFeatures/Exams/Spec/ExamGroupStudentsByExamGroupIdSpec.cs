namespace Athena.Application.Features.TeacherFeatures.Exams.Spec
{
    public class ExamGroupStudentsByExamGroupIdSpec : Specification<ExamGroupStudent>
    {
        public ExamGroupStudentsByExamGroupIdSpec(Guid Id) =>
            Query.Where(e => e.ExamGroupId == Id)
                .Include(e => e.GroupStudent.TeacherCourseLevelYearStudent.Student);
    }
}
