namespace Athena.Application.Features.TeacherFeatures.Years.Dtos
{
    public class TeacherCourseLevelYearSemsterDto : IDto
    {
        public Guid Id { get; set; }
        public string? Semster { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? State { get; set; }
    }
}
