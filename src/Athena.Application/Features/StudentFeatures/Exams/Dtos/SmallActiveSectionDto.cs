namespace Athena.Application.Features.StudentFeatures.Exams.Dtos
{
    public class SmallActiveSectionDto : IDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Paragraph { get; set; }
        public double Degree { get; set; }
        public bool IsPrime { get; set; }
        public bool HasMCQ { get; set; }
        public bool HasWritten { get; set; }
        public string? State { get; set; }

        /*public List<ActiveQuestionDto>? Questions { get; set; }
        public List<ActiveImageDetailDto>? Images { get; set; }*/
    }
}
