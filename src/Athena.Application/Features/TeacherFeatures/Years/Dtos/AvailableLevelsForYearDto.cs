namespace Athena.Application.Features.TeacherFeatures.Years.Dtos
{
    public class AvailableLevelsForYearDto : IDto
    {
        public Guid TeacherCourseLevelId { get; set; }
        public string? LevelName { get; set; }
    }
}
