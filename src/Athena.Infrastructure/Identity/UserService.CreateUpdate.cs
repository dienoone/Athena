using Athena.Application.Common.Exceptions;
using Athena.Application.Common.Mailing;
using Athena.Application.Identity.Users;
using Athena.Domain.Identity;
using Athena.Shared.Authorization;

namespace Athena.Infrastructure.Identity
{
    internal partial class UserService
    {
       
        public async Task<string> CreateAsync(CreateUserRequest request, string origin, string role, Guid? businessId)
        {
            var user = new ApplicationUser
            {
                Email = request.Email,
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                LastName = request.LastName,
                Gender = request.Gender,
                UserName = request.UserName,
                PhoneNumber = request.PhoneNumber,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded) throw new InternalServerException(_t["Validation Errors Occurred."], result.GetErrors(_t));
            
            switch(role)
            {
                case ARoles.Admin:
                    user.BusinessId = await _athenaAdmin.GetAthenBusinessId();
                    break;

                case ARoles.Teacher:
                    user.BusinessId = Guid.Parse(user.Id);
                    break;

                case ARoles.Employee:
                    user.BusinessId = (Guid)businessId!;
                    break;
            }
            await _userManager.AddToRoleAsync(user, role);

            // Send Message
            // TODO: Send Message
            if (_securitySettings.RequireConfirmedAccount && !string.IsNullOrEmpty(user.Email))
            {
                // send verification email
                await SendConfirmEmailAsync(user.Id, origin);
            }

            await _events.PublishAsync(new ApplicationUserCreatedEvent(user.Id));

            return user.Id.ToString();
        }

        public async Task SendConfirmEmailAsync(string userId, string origin)
        {
            var user = await _userManager.FindByIdAsync(userId);
            _ = user ?? throw new NotFoundException(_t["User Not Found!"]);

            string emailVerificationUri = await GetEmailVerificationUriAsync(user, origin);

            RegisterUserEmailModel eMailModel = new()
            {
                Email = user.Email,
                UserName = user.UserName,
                Url = emailVerificationUri
            };

            var mailRequest = new MailRequest(
                new List<string> { user.Email },
                _t["Confirm Registration"],
                _templateService.GenerateEmailTemplate("email-confirmation", eMailModel));

            _jobService.Enqueue(() => _mailService.SendAsync(mailRequest, CancellationToken.None));
        }

        public async Task UpdateAsync(UpdateUserRequest request, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            _ = user ?? throw new NotFoundException(_t["User Not Found."]);

           /* string currentImage = user.ImageUrl ?? string.Empty;
            if (request.Image != null || request.DeleteCurrentImage)
            {
                user.ImageUrl = await _fileStorage.UploadAsync<ApplicationUser>(request.Image, FileType.Image);
                if (request.DeleteCurrentImage && !string.IsNullOrEmpty(currentImage))
                {
                    string root = Directory.GetCurrentDirectory();
                    _fileStorage.Remove(Path.Combine(root, currentImage));
                }
            }*/

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.PhoneNumber = request.PhoneNumber;
            string phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (request.PhoneNumber != phoneNumber)
            {
                await _userManager.SetPhoneNumberAsync(user, request.PhoneNumber);
            }

            var result = await _userManager.UpdateAsync(user);

            await _signInManager.RefreshSignInAsync(user);

            await _events.PublishAsync(new ApplicationUserUpdatedEvent(user.Id));

            if (!result.Succeeded)
            {
                throw new InternalServerException(_t["Update profile failed"], result.GetErrors(_t));
            }
        }

    }
}
