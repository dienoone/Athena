namespace Athena.Application.Features.TeacherFeatures.Groups.Dtos
{
    public class GroupsRequiredDto : IDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
    }
}
