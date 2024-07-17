namespace Athena.Application.Features.StudentFeatures.Home.Dtos
{
    public class UpcommingExamDto : IDto
    {
        public Guid Id { get; set; }
        public string? Exam { get; set; }
        public string? Teacher { get; set; }
        public string? Image { get; set; }
        public string? State { get; set; }
        public string? Color { get; set; }

    }
}
