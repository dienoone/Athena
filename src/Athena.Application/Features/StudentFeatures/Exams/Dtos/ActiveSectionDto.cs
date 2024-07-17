namespace Athena.Application.Features.StudentFeatures.Exams.Dtos
{
    public class ActiveSectionDto : IDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Paragraph { get; set; }

        public List<ActiveQuestionDto>? Questions { get; set; }
        public List<ActiveImageDetailDto>? Images { get; set; }

    }
}
