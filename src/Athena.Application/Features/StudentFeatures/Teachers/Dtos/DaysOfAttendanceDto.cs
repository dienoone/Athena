namespace Athena.Application.Features.StudentFeatures.Teachers.Dtos
{
    public class DaysOfAttendanceDto : IDto
    {
        public Guid Id { get; set; }
        public string? Day { get; set; }
        public TimeSpan StartTime { get; set; }
    }
}
