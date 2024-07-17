using Athena.Application.Features.DashboardFeatures.Teachers.Commands;
using Athena.Application.Features.DashboardFeatures.Teachers.Dto;
using Athena.Application.Features.DashboardFeatures.Teachers.Queries;

namespace Athena.Api.Controllers.Dashboard
{
    public class TeachersController : VersionNeutralApiDashboardGroupController
    {
        /*[HttpGet("{id:guid}")]
        [AllowAnonymous]
        [OpenApiOperation("Get teacher details.", "")]
        public async Task<TeacherBaseDto> GetAsync(Guid id)
        {
            return await Mediator.Send(new GetTeacherByIdRequest(id));
        }*/

        [HttpGet("Base")]
        [OpenApiOperation("Get teacher base.", "")]
        public async Task<TeacherBaseDto> GetAsync()
        {
            return await Mediator.Send(new GetTeacherBaseRequest());
        }

        [HttpPost]
        [MustHavePermission(AAction.Create, AResource.Teachers)]
        [OpenApiOperation("Create a new teacher.", "")]
        public async Task<IActionResult> CreateAsync(CreateTeacherRequest request)
        {
            request.Origin = GetOriginFromRequest();
            return Ok(await Mediator.Send(request));
        }


        private string GetOriginFromRequest() => $"{Request.Scheme}://{Request.Host.Value}{Request.PathBase.Value}";
    }
}
