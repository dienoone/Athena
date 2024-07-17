namespace Athena.Application.Features.TeacherFeatures.Exams.Spec
{
    public class StudentSectionStatesByExamIdSpec : Specification<StudentSectionState>
    {
        public StudentSectionStatesByExamIdSpec(Guid examId) =>
            Query.Where(e => e.Section.ExamId == examId);
    }
}
