namespace Athena.Application.Features.TeacherFeatures.Exams.Dto
{
    public class ExamGroupDto : IDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
    }
}
