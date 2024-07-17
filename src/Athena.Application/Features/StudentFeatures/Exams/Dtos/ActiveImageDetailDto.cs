namespace Athena.Application.Features.StudentFeatures.Exams.Dtos
{
    public class ActiveImageDetailDto : IDto
    {
        public Guid Id { get; set; }
        public int Index { get; set; }
        public string? Image { get; set; }
    }
}
