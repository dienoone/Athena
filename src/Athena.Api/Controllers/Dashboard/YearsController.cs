using Athena.Application.Features.DashboardFeatures.Years.Commands;
using Athena.Application.Features.DashboardFeatures.Years.Dtos;
using Athena.Application.Features.DashboardFeatures.Years.Queries;

namespace Athena.Api.Controllers.Dashboard
{
    // ToDo: Add Authorization For Rules:
    public class YearsController : VersionNeutralApiDashboardGroupController
    {
        [HttpGet]
        [AllowAnonymous]
        [OpenApiOperation("Get Years.", "")]
        public async Task<YearsRequestDto> GetAsync()
        {
            return await Mediator.Send(new GetYearsReqesut());
        }

        [HttpPost]
        [AllowAnonymous]
        [OpenApiOperation("Create a new year.", "")]
        public async Task<Guid> CreateAsync(CreateDashboardYearRequest request)
        {
            return await Mediator.Send(request);
        }

        [HttpPut("{id:guid}")]
        [AllowAnonymous]
        [OpenApiOperation("Update a year.", "")]
        public async Task<ActionResult<Guid>> UpdateAsync(UpdateDashboardYearRequest request, Guid id)
        {
            return id != request.Id
          ? BadRequest()
          : Ok(await Mediator.Send(request));
        }

        [HttpPut("end/{id:guid}")]
        [AllowAnonymous]
        [OpenApiOperation("End a year.", "")]
        public async Task<Guid> EndAsync(Guid id)
        {
            return await Mediator.Send(new EndDashboardYearRequest(id));
        }
    }
}
