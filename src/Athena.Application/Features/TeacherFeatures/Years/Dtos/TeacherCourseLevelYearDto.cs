namespace Athena.Application.Features.TeacherFeatures.Years.Dtos
{
    public class TeacherCourseLevelYearDto : IDto
    {
        public Guid Id { get; set; }
        public Guid TeacherCourseLevelId { get; set; }
        public string? LevelName { get; set; }
        public int IntroFee { get; set; }
        public int MonthFee { get; set; }
        public bool State { get; set; }

        public List<TeacherCourseLevelYearSemsterDto>? Semsters { get; set; }
    }
}
