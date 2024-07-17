using Athena.Application.Identity.Tokens;
using Athena.Shared.Authorization;
using System.Text.Json.Serialization;

namespace Athena.Application.Features.TeacherFeatures.Auth
{
    public class RefreshTokenTeacherRequest : IRequest<TokenResponse>
    {
        public string Token { get; set; } = default!;
        public string RefreshToken { get; set; } = default!;

        [JsonIgnore]
        public string? IpAddress { get; set; }
    }

    public class RefreshTokenTeacherRequestHandler : IRequestHandler<RefreshTokenTeacherRequest, TokenResponse>
    {
        private readonly ITokenService _tokenService;

        public RefreshTokenTeacherRequestHandler(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }


        public async Task<TokenResponse> Handle(RefreshTokenTeacherRequest request, CancellationToken cancellationToken)
        {
            return await _tokenService.RefreshTokenAsync(new(request.Token, request.RefreshToken), ARoles.Student, request.IpAddress!);
        }
    }
}
