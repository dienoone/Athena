using Athena.Application.Features.DashboardFeatures.Levels.Commands;
using Athena.Application.Features.DashboardFeatures.Levels.Dtos;
using Athena.Application.Features.DashboardFeatures.Levels.Queries;

namespace Athena.Api.Controllers.Dashboard
{
    public class LevelsController : VersionNeutralApiDashboardGroupController
    {
        [HttpGet()]
        [AllowAnonymous]
        [OpenApiOperation("Get levels.", "")]
        public async Task<List<LevelDetailDto>> GetAsync()
        {
            return await Mediator.Send(new GetLevelsRequest());
        }

        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        [OpenApiOperation("Get level details.", "")]
        public async Task<LevelDetailDto> GetAsync(Guid id)
        {
            return await Mediator.Send(new GetLevelByIdRequest(id));
        }

        [HttpPost]
        [MustHavePermission(AAction.Create, AResource.Levels)]
        [OpenApiOperation("Create a new level.", "")]
        public async Task<Guid> CreateAsync(CreateLevelRequest request)
        {
            return await Mediator.Send(request);
        }

        [HttpPut("{id:guid}")]
        [MustHavePermission(AAction.Update, AResource.Levels)]
        [OpenApiOperation("Update a level.", "")]
        public async Task<ActionResult<Guid>> UpdateAsync(UpdateLevelRequest request, Guid id)
        {
            return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
        }

        [HttpDelete("{id:guid}")]
        [MustHavePermission(AAction.Delete, AResource.Levels)]
        [OpenApiOperation("Delete a level.", "")]
        public async Task<Guid> DeleteAsync(Guid id)
        {
            return await Mediator.Send(new DeleteLevelRequest(id));
        }

    }
}
