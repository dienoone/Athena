namespace Athena.Application.Features.StudentFeatures.TimeTable.Dtos
{
    public class TeacherGroupScheduleDto : IDto
    {
        public Guid Id { get; set; }
        public string? Image { get; set; }
        public string? Teacher { get; set; }
        public string? Course { get; set; }
        public Guid GroupId { get; set; }
        public string? GroupName { get; set; }

        public List<GroupScheduleDto>? Scaduals { get; set; }
    }
}
