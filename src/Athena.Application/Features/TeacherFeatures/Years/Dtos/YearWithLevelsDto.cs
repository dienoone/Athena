namespace Athena.Application.Features.TeacherFeatures.Years.Dtos
{
    public class LevelsRequiredDto : IDto
    {
        public Guid TeacherCourseLevelYearId { get; set; }
        public string? LevelName { get; set; }
    }
}
