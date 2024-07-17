using Athena.Application.Identity.Tokens;
using Athena.Application.Identity.Users;
using Athena.Shared.Authorization;
using System.Text.Json.Serialization;

namespace Athena.Application.Features.StudentFeatures.Base.Commands
{
    public class ResetPasswordStudentRequest : IRequest<TokenResponse>
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string Token { get; set; } = default!;

        [JsonIgnore]
        public string? IpAddress { get; set; }

    }

    public class ResetPasswordStudentRequestHandler : IRequestHandler<ResetPasswordStudentRequest, TokenResponse>
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IStringLocalizer<ResetPasswordStudentRequest> _t;

        public ResetPasswordStudentRequestHandler(
            IUserService userService, 
            ITokenService tokenService,
            IStringLocalizer<ResetPasswordStudentRequest> t)
        {
            _userService = userService;
            _tokenService = tokenService;
            _t = t;
        }

        public async Task<TokenResponse> Handle(ResetPasswordStudentRequest request, CancellationToken cancellationToken)
        {
            var result = await _userService.ResetPasswordAsync(request);

            if(!result)
                throw new InternalServerException(_t["An Error has occurred!"]);

            var token = await _tokenService.GetTokenAsync(new(request.Email, request.Password), ARoles.Student, request.IpAddress!, cancellationToken);

            return token;
        }
    }
}
