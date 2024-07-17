namespace Athena.Application.Features.StudentFeatures.Exams.Dtos
{
    public class UpcomingExamDto : IDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public DateTime PublishedDate { get; set; }
        public TimeSpan PublishedTime { get; set; }
    }
}
