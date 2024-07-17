using Athena.Application.Identity.Users;

namespace Athena.Application.Features.StudentFeatures.Base.Commands
{
    public record ConfirmEmailStudentReqesut(Guid UserId, string Code) : IRequest<Guid>;

    public class ConfirmEmailStudentReqesutHandler : IRequestHandler<ConfirmEmailStudentReqesut, Guid>
    {
        private readonly IUserService _userService;
        private readonly IStringLocalizer<ConfirmEmailStudentReqesutHandler> _t;

        public ConfirmEmailStudentReqesutHandler(IUserService userService, IStringLocalizer<ConfirmEmailStudentReqesutHandler> t)
        {
            _userService = userService;
            _t = t;

        }
        public async Task<Guid> Handle(ConfirmEmailStudentReqesut request, CancellationToken cancellationToken)
        {
            var result = await _userService.ConfirmEmailAsync(request.UserId.ToString(), request.Code, cancellationToken);

            if(!result)
                throw new InternalServerException(_t["An error occurred while confirming E-Mail."]);

            return request.UserId;
        }
    }
}
