using Athena.Application.Features.StudentFeatures.Base.Commands;
using Athena.Application.Features.StudentFeatures.Base.Dtos;
using Athena.Application.Identity.Users.Password;

namespace Athena.Application.Identity.Users
{
    // Finished CraeteOnly
    // ToDo: Forget password cycle works only for students needed to be generalized.
    public interface IUserService : ITransientService
    {
        Task<PaginationResponse<UserDetailsDto>> SearchAsync(UserListFilter filter, CancellationToken cancellationToken);

        Task<bool> ExistsWithNameAsync(string name);
        Task<bool> ExistsWithEmailAsync(string email, string? exceptId = null);
        Task<bool> ExistsWithPhoneNumberAsync(string phoneNumber, string? exceptId = null);

        Task<List<UserDetailsDto>> GetListAsync(CancellationToken cancellationToken);

        Task<int> GetCountAsync(CancellationToken cancellationToken);

        Task<UserDetailsDto> GetAsync(string userId, CancellationToken cancellationToken);
        Task<UserDetailsDto?> GetByEmailAsync(string email, CancellationToken cancellationToken);

        Task<List<string>> GetRolesAsync(string userId, CancellationToken cancellationToken);
        Task<string> AssignRolesAsync(string userId, UserRolesRequest request, CancellationToken cancellationToken);

        Task<List<string>> GetPermissionsAsync(string userId, CancellationToken cancellationToken);
        Task<bool> HasPermissionAsync(string userId, string permission, CancellationToken cancellationToken = default);
        Task InvalidatePermissionCacheAsync(string userId, CancellationToken cancellationToken);

        Task ToggleStatusAsync(ToggleUserStatusRequest request, CancellationToken cancellationToken);

        Task<string> CreateAsync(CreateUserRequest request, string origin, string role, Guid? businessId);
        Task UpdateAsync(UpdateUserRequest request, string userId);

        Task<bool> ConfirmEmailAsync(string userId, string code, CancellationToken cancellationToken); 
        Task<string> ConfirmPhoneNumberAsync(string userId, string code);

        Task<string> ForgotPasswordAsync(ForgotPasswordStudentRequest request, string origin);
        Task<bool> ResetPasswordAsync(ResetPasswordStudentRequest request);
        Task ChangePasswordAsync(ChangePasswordRequest request, string userId);

        Task<string> VerifyOTPCodeAsync(VerifyOTPCodeStudentRequest request, CancellationToken cancellationToken);
        Task SendConfirmEmailAsync(string userId, string origin);
        Task SendOTPCodeAsync(string email, OTPStudentModel otpModel);
    }
}
