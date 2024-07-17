using Athena.Application.Features.TeacherFeatures.Groups.Dtos;

namespace Athena.Application.Features.TeacherFeatures.HeadQuarters.Dtos
{
    public class HeadQuarterLevelsDto : IDto
    {
        public Guid Id { get; set; }
        public string? LevelName { get; set; }
        public List<GroupsRequiredDto>? Groups { get; set; }
    }
}
