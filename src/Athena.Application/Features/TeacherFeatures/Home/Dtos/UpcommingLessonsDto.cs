namespace Athena.Application.Features.TeacherFeatures.Home.Dtos
{
    public class UpcommingLessonsDto : IDto
    {
        public string? GroupName { get; set; }
        public string? HeadQuarterName { get; set; }
        public TimeSpan Time { get; set; }
    }
}
