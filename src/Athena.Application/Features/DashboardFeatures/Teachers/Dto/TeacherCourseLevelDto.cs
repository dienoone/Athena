namespace Athena.Application.Features.DashboardFeatures.Teachers.Dto
{
    public class TeacherCourseLevelDto : IDto
    {
        public Guid TeacherCourseLevelId { get; set; }
        public string LevelName { get; set; } = default!;
    }
}
