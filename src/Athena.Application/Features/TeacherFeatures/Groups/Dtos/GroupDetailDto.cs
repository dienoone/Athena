namespace Athena.Application.Features.TeacherFeatures.Groups.Dtos
{
    public class GroupDetailDto : IDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public Guid TeacherCourseLevelId { get; set; }
        public string? Level { get; set; }
        public Guid HeadQuarterId { get; set; }
        public string? HeadQuarter { get; set; }
        public int Limit { get; set; }
        public int StudentsCount { get; set; }
        public string? YearState { get; set; }

        public List<GroupScadualDto>? GroupScaduals { get; set; }
        public List<GroupStudentDto>? GroupStudents { get; set; }
    }
}
