namespace Athena.Application.Features.StudentFeatures.Teachers.Dtos
{
    public class TeacherTuitionYearDetailsDto : IDto
    {
        public Guid Id { get; set; }
        public string? State { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public List<TeacherTuitionYearLevelDetailsDto>? Levels { get; set; }
    }
}
