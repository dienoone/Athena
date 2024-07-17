namespace Athena.Application.Features.TeacherFeatures.Exams.Commands.Helpers
{
    public class CreateExamHandlerHelper
    {
        public List<Guid> ExamGroupStudentIds { get; set; } = default!;
        public List<Guid> StudentIds { get; set; } = default!;
    }
}
