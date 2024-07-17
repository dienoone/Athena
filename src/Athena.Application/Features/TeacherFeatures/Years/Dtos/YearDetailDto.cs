namespace Athena.Application.Features.TeacherFeatures.Years.Dtos
{
    public class YearDetailDto : IDto
    {
        public Guid Id { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool State { get; set; }
        public string? YearState { get; set; }

        public List<TeacherCourseLevelYearDto>? Levels { get; set; }
    }
}
