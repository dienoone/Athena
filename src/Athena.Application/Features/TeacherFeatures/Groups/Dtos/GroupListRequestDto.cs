namespace Athena.Application.Features.TeacherFeatures.Groups.Dtos
{
    public class GroupListRequestDto
    {
        public List<GroupListDto>? Open { get; set; }
        public List<GroupListDto>? PreOpen { get; set; }
    }
}
