using Athena.Application.Features.StudentFeatures.Auth;
using Athena.Application.Identity.Tokens;

namespace Athena.Api.Controllers.Student
{
    public class AuthController : VersionNeutralApiStudentGroupController
    {
        [HttpPost]
        [AllowAnonymous]
        [OpenApiOperation("Request an access token using credentials.", "")]
        public async Task<TokenResponse> GetTokenAsync(LoginStudentRequest request)
        {
            request.IpAddress = GetIpAddress();
            return await Mediator.Send(request);
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        [OpenApiOperation("Request an access token using a refresh token.", "")]
        public async Task<TokenResponse> RefreshAsync(RefreshTokenStudentRequest request)
        {
            request.IpAddress = GetIpAddress();
            return await Mediator.Send(request);
        }

        private string GetIpAddress() =>
            Request.Headers.ContainsKey("X-Forwarded-For")
                ? Request.Headers["X-Forwarded-For"]
                : HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "N/A";
    }
}
