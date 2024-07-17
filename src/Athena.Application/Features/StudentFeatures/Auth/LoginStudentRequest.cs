using Athena.Application.Identity.Tokens;
using Athena.Shared.Authorization;
using System.Text.Json.Serialization;

namespace Athena.Application.Features.StudentFeatures.Auth
{
    public class LoginStudentRequest : IRequest<TokenResponse>
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;

        [JsonIgnore]
        public string? IpAddress { get; set; }
    }

    public class LoginStudentRequestHandler : IRequestHandler<LoginStudentRequest, TokenResponse>
    {
        private readonly ITokenService _tokenService;
        public LoginStudentRequestHandler(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async Task<TokenResponse> Handle(LoginStudentRequest request, CancellationToken cancellationToken)
        {
            return await _tokenService.GetTokenAsync(new(request.Email, request.Password), ARoles.Student, request.IpAddress!, cancellationToken);
        }
    }
}
