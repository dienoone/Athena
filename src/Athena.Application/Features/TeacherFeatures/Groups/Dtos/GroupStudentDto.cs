namespace Athena.Application.Features.TeacherFeatures.Groups.Dtos
{
    public class GroupStudentDto : IDto
    {
        public Guid Id { get; set; }
        public string? Image { get; set; }
        public string? FullName { get; set; }
        public string? Level { get; set; }
        public string? Code { get; set; }
    }
}