using Athena.Application.Features.DashboardFeatures.ExamTypes.Commands;
using Athena.Application.Features.DashboardFeatures.ExamTypes.Dtos;
using Athena.Application.Features.DashboardFeatures.ExamTypes.Queries;

namespace Athena.Api.Controllers.Dashboard
{
    public class ExamTypesController : VersionNeutralApiDashboardGroupController
    {
        [HttpGet]
        [AllowAnonymous]
        [OpenApiOperation("Get ExamTypes.", "")]
        public async Task<List<ExamTypeDto>> GetAsync()
        {
            return await Mediator.Send(new GetExamTypesRequest());
        }

        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        [OpenApiOperation("Get ExamType details.", "")]
        public async Task<ExamTypeDto> GetAsync(Guid id)
        {
            return await Mediator.Send(new GetExamTypeByIdRequest(id));
        }

        [HttpPost]
        [MustHavePermission(AAction.Create, AResource.ExamType)]
        [OpenApiOperation("Create a new ExamType.", "")]
        public async Task<Guid> CreateAsync(CreateExamTypeRequest request)
        {
            return await Mediator.Send(request);
        }

        [HttpPut("{id:guid}")]
        [MustHavePermission(AAction.Update, AResource.ExamType)]
        [OpenApiOperation("Update a ExamType.", "")]
        public async Task<ActionResult<Guid>> UpdateAsync(UpdateExamTypeRequest request, Guid id)
        {
            return id != request.Id
           ? BadRequest()
           : Ok(await Mediator.Send(request));
        }


        [HttpDelete("{id:guid}")]
        [MustHavePermission(AAction.Delete, AResource.ExamType)]
        [OpenApiOperation("Delete a ExamType.", "")]
        public async Task<Guid> DeleteAsync(Guid id)
        {
            return await Mediator.Send(new DeleteExamTypeRequest(id));
        }
    }
}
