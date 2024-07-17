namespace Athena.Application.Features.StudentFeatures.TimeTable.Dtos
{
    public class DayScheduleDto : IDto
    {
        public Guid GroupId { get; set; }
        public string? GroupName { get; set; }
        public string? Course { get; set; }
        public string? Teacher { get; set; }
        public string? Image { get; set; }
        public TimeSpan StartTime { get; set; }
        public string? Day { get; set; }
    }
}
