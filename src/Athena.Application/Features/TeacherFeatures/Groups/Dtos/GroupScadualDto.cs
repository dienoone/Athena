namespace Athena.Application.Features.TeacherFeatures.Groups.Dtos
{
    public class GroupScadualDto : IDto
    {
        public Guid Id { get; set; }
        public string? Day { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

    }
}
