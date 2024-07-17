namespace Athena.Application.Features.StudentFeatures.Teachers.Dtos
{
    public class TeacherTimeTableDto : IDto
    {
        public Guid GroupId { get; set; }
        public string? GroupName { get; set; }
        public List<DaysOfAttendanceDto>? DaysOfAttendances { get; set; }
    }
}
