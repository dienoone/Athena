using Athena.Application.Features.StudentFeatures.Profile.Commands;
using Athena.Application.Features.StudentFeatures.Profile.Dtos;
using Athena.Application.Features.StudentFeatures.Profile.Queries;

namespace Athena.Api.Controllers.Student
{
    public class AccountController : VersionNeutralApiStudentGroupController
    {
        [HttpGet]
        [OpenApiOperation("get account Details.", "")]
        public async Task<StudentProfileDto> GetAsync()
        {
            return await Mediator.Send(new GetStudentProfileRequest());
        }

        [HttpPut("Image")]
        [OpenApiOperation("update image.", "")]
        public async Task<Guid> UpdateImageAsync(UpdateStudentImageRequest request)
        {
            return await Mediator.Send(request);
        }

        [HttpPut("personaldetails")]
        [OpenApiOperation("update personal details.", "")]
        public async Task<Guid> UpdatePersonalDetailsAsync(UpdateStudentPersonalDetailsRequest request)
        {
            return await Mediator.Send(request);
        }

        [HttpPut("parentdetails")]
        [OpenApiOperation("update parent details.", "")]
        public async Task<Guid> UpdateParnetDetailsAsync(UpdateStudentParentsDetailsRequest request)
        {
            return await Mediator.Send(request);
        }

        [HttpPut("educationdetails")]
        [OpenApiOperation("update education details.", "")]
        public async Task<Guid> UpdateEducationDetailsAsync(UpdateStudentEducationDetailsRequest request)
        {
            return await Mediator.Send(request);
        }
        
        [HttpPut("contactdetails")]
        [OpenApiOperation("update contact details.", "")]
        public async Task<Guid> UpdateContactDetailsAsync(UpdateStudentContactDetailsRequest request)
        {
            return await Mediator.Send(request);
        }

        [HttpPut("accountsettings")]
        [OpenApiOperation("update account settings.", "")]
        public async Task<Guid> UpdateAccountSettingsAsync(UpdateStudentAccountSettingsRequest request)
        {
            return await Mediator.Send(request);
        }
    }
}
