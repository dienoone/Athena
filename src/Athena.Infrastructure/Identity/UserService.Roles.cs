using Athena.Application.Common.Exceptions;
using Athena.Application.Identity.Users;
using Athena.Domain.Identity;
using Athena.Shared.Authorization;

namespace Athena.Infrastructure.Identity
{
    internal partial class UserService
    {
        public async Task<List<string>> GetRolesAsync(string userId, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return (List<string>)await _userManager.GetRolesAsync(user);
        }

        public async Task<string> AssignRolesAsync(string userId, UserRolesRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            var user = await _userManager.Users.Where(u => u.Id == userId).FirstOrDefaultAsync(cancellationToken);

            _ = user ?? throw new NotFoundException(_t["User Not Found."]);

            // Check if the user is an admin for which the admin role is getting disabled
            /*if (await _userManager.IsInRoleAsync(user, ARoles.Admin)
                && request.UserRoles.Any(a => !a.Enabled && a.RoleName == ARoles.Admin))
            {
                // Get count of users in Admin Role
                int adminCount = (await _userManager.GetUsersInRoleAsync(ARoles.Admin)).Count;

                // Check if user is not Root Tenant Admin
                // Edge Case : there are chances for other tenants to have users with the same email as that of Root Tenant Admin. Probably can add a check while User Registration
                *//*if (user.Email == MultitenancyConstants.Root.EmailAddress)
                {
                    if (_currentTenant.Id == MultitenancyConstants.Root.Id)
                    {
                        throw new ConflictException(_t["Cannot Remove Admin Role From Root Tenant Admin."]);
                    }
                }
                else if (adminCount <= 2)
                {
                    throw new ConflictException(_t["Tenant should have at least 2 Admins."]);
                }*//*
            }*/

            foreach (var userRole in request.UserRoles)
            {
                // Check if Role Exists
                if (await _roleManager.FindByNameAsync(userRole.RoleName) is not null)
                {
                    if (userRole.Enabled)
                    {
                        if (!await _userManager.IsInRoleAsync(user, userRole.RoleName))
                        {
                            await _userManager.AddToRoleAsync(user, userRole.RoleName);
                        }
                    }
                    else
                    {
                        await _userManager.RemoveFromRoleAsync(user, userRole.RoleName);
                    }
                }
            }

            await _events.PublishAsync(new ApplicationUserUpdatedEvent(user.Id, true));

            return _t["User Roles Updated Successfully."];
        }
    }
}
