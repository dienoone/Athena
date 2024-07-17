using Athena.Application.Identity.Tokens;
using Athena.Shared.Authorization;
using System.Text.Json.Serialization;

namespace Athena.Application.Features.TeacherFeatures.Auth
{
    public class LoginTeacherRequest : IRequest<TokenResponse>
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;

        [JsonIgnore]
        public string? IpAddress { get; set; }
    }

    public class LoginTeacherRequestHandler : IRequestHandler<LoginTeacherRequest, TokenResponse>
    {
        private readonly ITokenService _tokenService;
        public LoginTeacherRequestHandler(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }


        public async Task<TokenResponse> Handle(LoginTeacherRequest request, CancellationToken cancellationToken)
        {
            return await _tokenService.GetTokenAsync(new(request.Email, request.Password), ARoles.Teacher, request.IpAddress!, cancellationToken);
        }
    }
}
