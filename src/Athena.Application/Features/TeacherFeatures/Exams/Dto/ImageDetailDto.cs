namespace Athena.Application.Features.TeacherFeatures.Exams.Dto
{
    public class ImageDetailDto : IDto
    {
        public Guid Id { get; set; }
        public int Index { get; set; }
        public string? Image { get; set; }
    }
}
