namespace Athena.Application.Features.StudentFeatures.Home.Dtos
{
    // ToDo: Courses Colors:
    public class NextClassDto : IDto
    {
        public string? Course { get; set; }
        public string? TeacherName { get; set; }
        public TimeSpan? Time { get; set; }
        public string? Image { get; set; }
        public string? Color { get; set; }

    }
}
