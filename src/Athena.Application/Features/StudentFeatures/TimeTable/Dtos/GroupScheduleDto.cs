namespace Athena.Application.Features.StudentFeatures.TimeTable.Dtos
{
    public class GroupScheduleDto : IDto
    {
        public Guid Id { get; set; }
        public string? Day { get; set; }
        public TimeSpan StartTime { get; set; }
    }
}
