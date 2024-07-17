namespace Athena.Application.Features.StudentFeatures.Exams.Dtos
{
    public class AnswerActiveSectionDto : IDto
    {
        public Guid Id { get; set; }
        public string? State { get; set; }
    }
}
