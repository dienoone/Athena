using Athena.Application.Features.TeacherFeatures.Auth;
using Athena.Application.Identity.Tokens;

namespace Athena.Api.Controllers.Teacher
{
    public class AuthController : VersionNeutralApiTeacherGroupController
    {
        [HttpPost]
        [AllowAnonymous]
        [OpenApiOperation("Request an access token using credentials.", "")]
        public async Task<TokenResponse> GetTokenAsync(LoginTeacherRequest request)
        {
            request.IpAddress = GetIpAddress();
            return await Mediator.Send(request);
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        [OpenApiOperation("Request an access token using a refresh token.", "")]
        public async Task<TokenResponse> RefreshAsync(RefreshTokenTeacherRequest request)
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
