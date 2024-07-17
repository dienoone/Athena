using Athena.Application.Common.Exceptions;
using Athena.Application.Common.Mailing;
using Athena.Application.Features.StudentFeatures.Base.Commands;
using Athena.Application.Features.StudentFeatures.Base.Dtos;
using Athena.Application.Identity.Users.Password;

namespace Athena.Infrastructure.Identity
{
    internal partial class UserService
    {
        public async Task<string> ForgotPasswordAsync(ForgotPasswordStudentRequest request, string origin)
        {
            EnsureValidTenant();

            var user = await _userManager.FindByEmailAsync(request.Email.Normalize());
            if (user is null)
            {
                // Don't reveal that the user does not exist or is not confirmed
                throw new NotFoundException(_t["User Not Found."]);
            }

            if(!await _userManager.IsEmailConfirmedAsync(user))
            {
                string emailVerificationUri = await GetEmailVerificationUriAsync(user, origin);

                RegisterUserEmailModel eMailModel = new RegisterUserEmailModel()
                {
                    Email = user.Email,
                    UserName = user.UserName,
                    Url = emailVerificationUri
                };

                var emailRequest = new MailRequest(
                    new List<string> { user.Email },
                    _t["Confirm Registration"],
                    _templateService.GenerateEmailTemplate("email-confirmation", eMailModel));

                _jobService.Enqueue(() => _mailService.SendAsync(emailRequest, CancellationToken.None));

                throw new UnauthorizedException(_t["Email Not Confirmed."]);
            }

            // For more information on how to enable account confirmation and password reset please
            // visit https://go.microsoft.com/fwlink/?LinkID=532713
            string code = await _userManager.GeneratePasswordResetTokenAsync(user);
            return code;
        }

        public async Task<string> VerifyOTPCodeAsync(VerifyOTPCodeStudentRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email.Normalize());
            _ = user ?? throw new NotFoundException(_t["User Not Found!"]);

            // For more information on how to enable account confirmation and password reset please
            // visit https://go.microsoft.com/fwlink/?LinkID=532713
            string code = await _userManager.GeneratePasswordResetTokenAsync(user);
            return code;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordStudentRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email?.Normalize());

            // Don't reveal that the user does not exist
            _ = user ?? throw new InternalServerException(_t["An Error has occurred!"]);

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);

            return result.Succeeded;
        }

        public async Task ChangePasswordAsync(ChangePasswordRequest model, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            _ = user ?? throw new NotFoundException(_t["User Not Found."]);

            var result = await _userManager.ChangePasswordAsync(user, model.Password, model.NewPassword);

            if (!result.Succeeded)
            {
                throw new InternalServerException(_t["Change password failed"], result.GetErrors(_t));
            }
        }

        public async Task SendOTPCodeAsync(string email, OTPStudentModel otpModel)
        {
            var user = await _userManager.FindByEmailAsync(email);

            var mailRequest = new MailRequest(
                new List<string> { email },
                _t["Reset Password"],
                _templateService.GenerateEmailTemplate("otp-code", otpModel));

            _jobService.Enqueue(() => _mailService.SendAsync(mailRequest, CancellationToken.None));
        }
    }
}
