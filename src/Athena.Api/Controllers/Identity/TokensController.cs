using Athena.Application.Identity.Tokens;

namespace Athena.Api.Controllers.Identity
{
    public sealed class TokensController : VersionNeutralApiAuthGroupController
    {
        private readonly ITokenService _tokenService;

        public TokensController(ITokenService tokenService) => _tokenService = tokenService;

        [HttpPost]
        [AllowAnonymous]
        [OpenApiOperation("Request an access token using credentials.", "")]
        public Task<TokenResponse> GetTokenAsync(TokenRequest request, CancellationToken cancellationToken)
        {
            return _tokenService.GetTokenAsync(request, ARoles.SuperAdmin, GetIpAddress(), cancellationToken);
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        [OpenApiOperation("Request an access token using a refresh token.", "")]
        public Task<TokenResponse> RefreshAsync(RefreshTokenRequest request)
        {
            return _tokenService.RefreshTokenAsync(request, ARoles.SuperAdmin, GetIpAddress());
        }

        private string GetIpAddress() =>
            Request.Headers.ContainsKey("X-Forwarded-For")
                ? Request.Headers["X-Forwarded-For"]
                : HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "N/A";
    }
}
