using Athena.Application.Identity.Tokens;
using Athena.Shared.Authorization;
using System.Text.Json.Serialization;

namespace Athena.Application.Features.StudentFeatures.Auth
{
    public class RefreshTokenStudentRequest : IRequest<TokenResponse>
    {
        public string Token { get; set; } = default!;
        public string RefreshToken { get; set; } = default!;

        [JsonIgnore]
        public string? IpAddress { get; set; }
    }

    public class RefreshTokenStudentRequestHandler : IRequestHandler<RefreshTokenStudentRequest, TokenResponse>
    {
        private readonly ITokenService _tokenService;

        public RefreshTokenStudentRequestHandler(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async Task<TokenResponse> Handle(RefreshTokenStudentRequest request, CancellationToken cancellationToken)
        {
            return await _tokenService.RefreshTokenAsync(new(request.Token, request.RefreshToken), ARoles.Student, request.IpAddress!);
        }
    }
}
