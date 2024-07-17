using Athena.Application.Features.TeacherFeatures.Home.Dtos;
using Athena.Application.Features.TeacherFeatures.Home.Queries;

namespace Athena.Api.Controllers.Teacher
{
    public class HomeController : VersionNeutralApiTeacherGroupController
    {
        [HttpGet]
        [OpenApiOperation("Get home-request.")]
        public async Task<TeacherHomeRequestDto> GetAsync()
        {
            return await Mediator.Send(new GetTeacherHomeRequest());
        }

    }
}
