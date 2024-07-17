using Athena.Application.Features.TeacherFeatures.HeadQuarters.Dtos;
using Athena.Application.Features.TeacherFeatures.Years.Dtos;

namespace Athena.Application.Features.TeacherFeatures.Groups.Dtos
{
    public class GroupRequiredRequestDto : IDto
    {
        public List<HeadQuaerteRequiredDto>? HeadQuaertes { get; set; }
        public List<LevelsRequiredDto>? Open { get; set; }
        public List<LevelsRequiredDto>? Preopen { get; set; }

    }

    
}
