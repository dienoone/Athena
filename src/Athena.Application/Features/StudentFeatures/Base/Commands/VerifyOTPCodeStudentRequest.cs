using Athena.Application.Features.StudentFeatures.Base.Dtos;
using Athena.Application.Features.StudentFeatures.Base.Spec;
using Athena.Application.Identity.Users;

namespace Athena.Application.Features.StudentFeatures.Base.Commands
{
    public record VerifyOTPCodeStudentRequest(string Email, string Code) : IRequest<VerifyOTPCodeStudentDto>;

    public class VerifyOTPCodeStudentRequestHandler : IRequestHandler<VerifyOTPCodeStudentRequest, VerifyOTPCodeStudentDto>
    {
        private readonly IUserService _userService;
        private readonly IRepository<ResetPasswordToken> _resetPasswordTokenRepo;
        private readonly IStringLocalizer<ForgotPasswordRequestHandler> _t;

        public VerifyOTPCodeStudentRequestHandler(
            IUserService userService, 
            IRepository<ResetPasswordToken> resetPasswordTokenRepo, 
            IStringLocalizer<ForgotPasswordRequestHandler> t)
        {
            _userService = userService;
            _resetPasswordTokenRepo = resetPasswordTokenRepo;
            _t = t;
        }

        public async Task<VerifyOTPCodeStudentDto> Handle(VerifyOTPCodeStudentRequest request, CancellationToken cancellationToken)
        {
            TimeZoneInfo egyptTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
            var dateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, egyptTimeZone);

            var otpToken = await _resetPasswordTokenRepo.GetBySpecAsync(new ResetPasswordTokenByCodeAndEmailSpec(request.Email, request.Code, dateTime), cancellationToken);
            _ = otpToken ?? throw new InternalServerException(_t["Invalid or expired OTP."]);

            var token = await _userService.VerifyOTPCodeAsync(request, cancellationToken);

            otpToken.Update(true);
            await _resetPasswordTokenRepo.UpdateAsync(otpToken, cancellationToken);

            return new()
            {
                Email = request.Email,
                Token = token
            };
        }
    }
}
