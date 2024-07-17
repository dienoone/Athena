using Athena.Application.Features.TeacherFeatures.Profile.Commands;
using Athena.Application.Features.TeacherFeatures.Profile.Dtos;
using Athena.Application.Features.TeacherFeatures.Profile.Queries;

namespace Athena.Api.Controllers.Teacher
{
    public class ProfileController : VersionNeutralApiTeacherGroupController
    {

        [HttpGet]
        [OpenApiOperation("Get teacher profile.", "")]
        public async Task<ProfileTeacherDto> GetAsync()
        {
            return await Mediator.Send(new GetTeacherProfileReqeust());
        }

        [HttpPut]
        [MustHavePermission(AAction.Update, AResource.Groups)]
        [OpenApiOperation("Update a teacher profile.", "")]
        public async Task<Guid> UpdateAsync(UpdateTeacherProfileRequest request)
        {
            return await Mediator.Send(request);
        }
    }
}
