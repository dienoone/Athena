using Athena.Application.Features.DashboardFeatures.Classification;
using Athena.Application.Features.DashboardFeatures.Classification.Commands;
using Athena.Application.Features.DashboardFeatures.Classification.Dtos;
using Athena.Application.Features.DashboardFeatures.Classification.Queries;

namespace Athena.Api.Controllers.Dashboard
{
    public class ClassificationsController : VersionNeutralApiDashboardGroupController
    {
        [HttpGet]
        [AllowAnonymous]
        [OpenApiOperation("Get Classifications.", "")]
        public async Task<List<ClassificationDto>> GetAsync()
        {
            return await Mediator.Send(new GetClassificationsRequest());
        }

        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        [OpenApiOperation("Get Classification details.", "")]
        public async Task<ClassificationDto> GetAsync(Guid id)
        {
            return await Mediator.Send(new GetClassificationByIdRequest(id));
        }

        [HttpPost]
        [MustHavePermission(AAction.Create, AResource.Classifications)]
        [OpenApiOperation("Create a new Classification.", "")]
        public async Task<Guid> CreateAsync(CreateClassificationRequest request)
        {
            return await Mediator.Send(request);
        }

        [HttpPut("{id:guid}")]
        [MustHavePermission(AAction.Update, AResource.Classifications)]
        [OpenApiOperation("Update a Classification.", "")]
        public async Task<ActionResult<Guid>> UpdateAsync(UpdateClassificationRequest request, Guid id)
        {
            return id != request.Id
          ? BadRequest()
          : Ok(await Mediator.Send(request));
        }


        [HttpDelete("{id:guid}")]
        [MustHavePermission(AAction.Delete, AResource.Classifications)]
        [OpenApiOperation("Delete a Classification.", "")]
        public async Task<Guid> DeleteAsync(Guid id)
        {
            return await Mediator.Send(new DeleteClassificationRequest(id));
        }
    }
}
