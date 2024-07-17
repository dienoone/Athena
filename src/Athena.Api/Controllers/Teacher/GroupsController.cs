using Athena.Application.Features.TeacherFeatures.Groups.Commands;
using Athena.Application.Features.TeacherFeatures.Groups.Dtos;
using Athena.Application.Features.TeacherFeatures.Groups.Queries;

namespace Athena.Api.Controllers.Teacher
{
    public class GroupsController : VersionNeutralApiTeacherGroupController
    {
        [HttpGet("required")]
        [OpenApiOperation("Get HeadQuarters-Yearlevels.", "")]
        public async Task<GroupRequiredRequestDto> GetRequiredAsync()
        {
            return await Mediator.Send(new GetGroupRequiredRequest());
        }

        [HttpGet]
        [OpenApiOperation("Get Groups.", "")]
        public async Task<GroupListRequestDto> GetAsync()
        {
            return await Mediator.Send(new GetGroupListRequest());
        }

        [HttpGet("{id:guid}")]
        [OpenApiOperation("Get Group Detail.", "")]
        public async Task<GroupDetailDto> GetAsync(Guid id)
        {
            return await Mediator.Send(new GetGroupDetailByIdRequest(id));
        }

        [HttpPost]
        [MustHavePermission(AAction.Create, AResource.Groups)]
        [OpenApiOperation("Create New Group.", "")]
        public async Task<Guid> CreateAsync(CreateGroupRequest reqeust)
        {
            return await Mediator.Send(reqeust);
        }

        [HttpPut("{id:guid}")]
        [MustHavePermission(AAction.Update, AResource.Groups)]
        [OpenApiOperation("Update a group.", "")]
        public async Task<ActionResult<Guid>> UpdateAsync(UpdateGroupRequest request, Guid id)
        {
            return id != request.Id
                ? BadRequest()
                : Ok(await Mediator.Send(request));
        }

        [HttpDelete("{id:guid}")]
        [MustHavePermission(AAction.Delete, AResource.Groups)]
        [OpenApiOperation("Delete a Group.", "")]
        public async Task<Guid> DeleteAsync(Guid id)
        {
            return await Mediator.Send(new DeleteGroupReqeust(id));
        }
    }
}
