using Athena.Application.Features.StudentFeatures.Base.Dtos;
using Athena.Application.Identity.Users;
using System.Security.AccessControl;
using System.Text.Json.Serialization;

namespace Athena.Application.Features.StudentFeatures.Base.Commands
{
    public class ForgotPasswordStudentRequest : IRequest<ForgotPasswordDto>
    {
        public string Email { get; set; } = default!;


        [JsonIgnore]
        public string? Origin { get; set; } = default!;
    }

    public class ForgotPasswordRequestValidator : CustomValidator<ForgotPasswordStudentRequest>
    {
        public ForgotPasswordRequestValidator(IStringLocalizer<ForgotPasswordRequestValidator> T) =>
            RuleFor(p => p.Email).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .EmailAddress()
                    .WithMessage(T["Invalid Email Address."]);
    }

    public class ForgotPasswordRequestHandler : IRequestHandler<ForgotPasswordStudentRequest, ForgotPasswordDto>
    {
        private readonly IUserService _userService;
        private readonly IRepository<ResetPasswordToken> _resetPasswordTokenRepo;
        private readonly IStringLocalizer<ForgotPasswordRequestHandler> _t;

        public ForgotPasswordRequestHandler(
            IUserService userService, 
            IRepository<ResetPasswordToken> resetPasswordTokenRepo, 
            IStringLocalizer<ForgotPasswordRequestHandler> t)
        {
            _userService = userService;
            _resetPasswordTokenRepo = resetPasswordTokenRepo;
            _t = t;
        }

        public async Task<ForgotPasswordDto> Handle(ForgotPasswordStudentRequest request, CancellationToken cancellationToken)
        {
            var user = await _userService.GetByEmailAsync(request.Email, cancellationToken);
            _ = user ?? throw new NotFoundException(_t["User {0} Not Found!", request.Email]);

            if (!user.EmailConfirmed)
            {
                await _userService.SendConfirmEmailAsync(user.Id.ToString(), request.Origin!);
                throw new UnauthorizedException(_t["Email Not Confirmed!"]);
            }

            // Generating a random numeric OTP (6 digits in this example)
            string otpCode = new Random().Next(100000, 999999).ToString();

            // Specify the desired time zone (Egypt Standard Time)
            TimeZoneInfo egyptTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
            var dateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, egyptTimeZone);

            ResetPasswordToken resetToken = new(request.Email, otpCode, dateTime.AddMinutes(15), false);
            await _resetPasswordTokenRepo.AddAsync(resetToken, cancellationToken);

            await _userService.SendOTPCodeAsync(request.Email, new() { Code = otpCode });

            return new()
            {
                Email = request.Email
            };
        }
    }
}
