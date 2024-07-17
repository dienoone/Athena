using Athena.Application.Features.StudentFeatures.Base.Commands;
using Athena.Application.Features.StudentFeatures.Base.Dtos;
using Athena.Application.Features.StudentFeatures.Students;
using Athena.Application.Features.StudentFeatures.Students.Dtos;
using Athena.Application.Identity.Tokens;

namespace Athena.Api.Controllers.Student
{
    public class BaseController : VersionNeutralApiStudentGroupController
    {
        [HttpGet]
        [OpenApiOperation("get student base.", "")]
        public async Task<StudentBaseDto> GetBaseAsync()
        {
            return await Mediator.Send(new GetStudentBaseRequest());
        }

        [HttpPost]
        [AllowAnonymous]
        [OpenApiOperation("Create a new student.", "")]
        public async Task<CreateStudnetResponseDto> CreateAsync(CreateStudentRequest request)
        {
            request.Origin = GetOriginFromRequest();
            return await Mediator.Send(request);
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        [OpenApiOperation("Request a password reset email for a user.", "")]
        public async Task<ForgotPasswordDto> ForgotPasswordAsync(ForgotPasswordStudentRequest request)
        {
            request.Origin = GetOriginFromRequest();
            return await Mediator.Send(request);
        }
        
        [HttpPost("verify-otp")]
        [AllowAnonymous]
        [OpenApiOperation("verify otp for reset password.", "")]
        public async Task<VerifyOTPCodeStudentDto> VerifyOTPAsync(VerifyOTPCodeStudentRequest request)
        {
            return await Mediator.Send(request);
        }

        // ResetPasswordStudentRequest
        [HttpPost("reset-password")]
        [AllowAnonymous]
        [OpenApiOperation("Reset a students's password.", "")]
        public async Task<TokenResponse> ResetPasswordAsync(ResetPasswordStudentRequest request)
        {
            request.IpAddress = GetIpAddress();
            return await Mediator.Send(request);
        }

        [HttpPost("confirm-email")]
        [AllowAnonymous]
        [OpenApiOperation("Confirm email address for a student.", "")]
        public async Task<Guid> ConfirmEmailAsync(ConfirmEmailStudentReqesut reqesut)
        {
            return await Mediator.Send(reqesut);
        }

        private string GetOriginFromRequest() => $"{Request.Scheme}://{Request.Host.Value}{Request.PathBase.Value}";

        private string GetIpAddress() =>
            Request.Headers.ContainsKey("X-Forwarded-For")
                ? Request.Headers["X-Forwarded-For"]
                : HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "N/A";
    }
}
