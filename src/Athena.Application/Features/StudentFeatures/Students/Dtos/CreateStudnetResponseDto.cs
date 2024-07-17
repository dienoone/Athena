namespace Athena.Application.Features.StudentFeatures.Students.Dtos
{
    public class CreateStudnetResponseDto : IDto
    {
        public Guid Id { get; set; }
        public string? Code { get; set; }
    }
}
