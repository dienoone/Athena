using Athena.Application.Features.StudentFeatures.Home.Dtos;
using Athena.Application.Features.StudentFeatures.Home.Queries;

namespace Athena.Api.Controllers.Student
{
    public class HomeController : VersionNeutralApiStudentGroupController
    {
        [HttpGet]
        [OpenApiOperation("Get Home Page", "")]
        public async Task<HomeRequestDto> GetListAsync()
        {
            return await Mediator.Send(new GetHomeRequest());
        }
    }
}
