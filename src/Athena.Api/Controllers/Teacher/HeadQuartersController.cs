using Athena.Application.Features.TeacherFeatures.HeadQuarters.Commands;
using Athena.Application.Features.TeacherFeatures.HeadQuarters.Dtos;
using Athena.Application.Features.TeacherFeatures.HeadQuarters.Queries;

namespace Athena.Api.Controllers.Teacher
{
    public class HeadQuartersController : VersionNeutralApiTeacherGroupController
    {
        [HttpGet]
        [OpenApiOperation("Get HeadQuarters.", "")]
        public async Task<List<HeadQuarterListDto>> GetAsync()
        {
            return await Mediator.Send(new GetHeadQuartersListRequest());
        }

        [HttpGet("{id:guid}")]
        [OpenApiOperation("Get HeadQuarter details.", "")]
        public async Task<HeadQuarterDetailDto> GetAsync(Guid id)
        {
            return await Mediator.Send(new GetHeadQuarterDetailByIdRequest(id));
        }

        [HttpPost]
        [MustHavePermission(AAction.Create, AResource.HeadQuarters)]
        [OpenApiOperation("Create a new HeadQuarter.", "")]
        public async Task<Guid> CreateAsync(CreateHeadQuarterRequest request)
        {
            return await Mediator.Send(request);
        }

        [HttpPut("{id:guid}")]
        [MustHavePermission(AAction.Update, AResource.HeadQuarters)]
        [OpenApiOperation("Update a HeadQuarter.", "")]
        public async Task<ActionResult<Guid>> UpdateAsync(UpdateHeaderQuarterRequest request, Guid id)
        {
            return id != request.Id
                ? BadRequest()
                : Ok(await Mediator.Send(request));
        }


        [HttpDelete("{id:guid}")]
        [MustHavePermission(AAction.Delete, AResource.HeadQuarters)]
        [OpenApiOperation("Delete a HeadQuarter.", "")]
        public async Task<Guid> DeleteAsync(Guid id)
        {
            return await Mediator.Send(new DeleteHeadQuarterRequest(id));
        }

    }
}
