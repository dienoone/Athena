using Athena.Application.Identity.Users;

namespace Athena.Application.Features.StudentFeatures.Profile.Commands
{
    public record UpdateStudentAccountSettingsRequest(string OldPassword, string NewPassword, string ConfirmPassword) : IRequest<Guid>;

    public class UpdateStudentAccountSettingsRequestHandler : IRequestHandler<UpdateStudentAccountSettingsRequest, Guid>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IUserService _userService;

        public UpdateStudentAccountSettingsRequestHandler(ICurrentUser currentUser, IUserService userService)
        {
            _currentUser = currentUser;
            _userService = userService;
        }

        public async Task<Guid> Handle(UpdateStudentAccountSettingsRequest request, CancellationToken cancellationToken)
        {
            await _userService.ChangePasswordAsync(new() { Password = request.OldPassword, NewPassword = request.NewPassword, ConfirmNewPassword = request.ConfirmPassword }, _currentUser.GetUserId().ToString());
            return _currentUser.GetUserId();
        }
    }
}
