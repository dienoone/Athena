namespace Athena.Application.Features.TeacherFeatures.Exams.Dto
{
    public class GroupsForCreateExamDto : IDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
    }
}
