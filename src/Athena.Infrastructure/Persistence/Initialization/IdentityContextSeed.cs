using Athena.Infrastructure.Identity;
using Athena.Infrastructure.Persistence.Context;
using Athena.Shared.Authorization;
using Athena.Shared.Business;
using Microsoft.AspNetCore.Identity;

namespace Athena.Infrastructure.Persistence.Initialization
{
    public static class IdentityContextSeed
    {
        public static async Task SeedDatabaseAsync(
            RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext dbContext,
            CancellationToken cancellationToken)
        {
            ApplicationUser superAdmin = await SeedSuperAdminUserAsync(userManager, dbContext, cancellationToken);
            if (superAdmin != null)
            {
                await SeedRolesAsync(roleManager, userManager, dbContext, superAdmin, cancellationToken);
            }
        }

        private static async Task SeedRolesAsync(
            RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext dbContext,
            ApplicationUser superAdmin,
            CancellationToken cancellationToken)
        {
            foreach (string roleName in ARoles.DefaultRoles)
            {
                if (await roleManager.Roles.SingleOrDefaultAsync(r => r.Name == roleName, cancellationToken: cancellationToken) is not ApplicationRole role)
                {
                    // Create the role
                    role = new ApplicationRole(roleName, $"{roleName} Role for {superAdmin.Id} Admin");
                    await roleManager.CreateAsync(role);
                }

                // Assign permissions
                if (roleName == ARoles.SuperAdmin)
                {
                    await AssignPermissionsToRoleAsync(roleManager, dbContext, APermissions.All, role, superAdmin, cancellationToken);
                    // Assgin role to user
                    if (!await userManager.IsInRoleAsync(superAdmin, ARoles.SuperAdmin))
                    {
                        await userManager.AddToRoleAsync(superAdmin, ARoles.SuperAdmin);
                    }
                }
                else if (roleName == ARoles.Admin)
                {
                    await AssignPermissionsToRoleAsync(roleManager, dbContext, APermissions.Admin, role, superAdmin, cancellationToken);
                    // Assgin role to user
                    if (!await userManager.IsInRoleAsync(superAdmin, ARoles.Admin))
                    {
                        await userManager.AddToRoleAsync(superAdmin, ARoles.Admin);
                    }
                }
                else if (roleName == ARoles.Teacher)
                {
                    await AssignPermissionsToRoleAsync(roleManager, dbContext, APermissions.Teacher, role, superAdmin, cancellationToken);
                }
                else if (roleName == ARoles.Employee)
                {
                    await AssignPermissionsToRoleAsync(roleManager, dbContext, APermissions.Employee, role, superAdmin, cancellationToken);
                }
                else if (roleName == ARoles.Student)
                {
                    await AssignPermissionsToRoleAsync(roleManager, dbContext, APermissions.Student, role, superAdmin, cancellationToken);
                }

            }
        }

        private static async Task AssignPermissionsToRoleAsync(
            RoleManager<ApplicationRole> roleManager,
            ApplicationDbContext dbContext,
            IReadOnlyList<APermission> permissions,
            ApplicationRole role,
            ApplicationUser admin,
            CancellationToken cancellationToken)
        {
            var currentClaims = await roleManager.GetClaimsAsync(role);
            foreach (var permission in permissions)
            {
                if (!currentClaims.Any(c => c.Type == AClaims.Permission && c.Value == permission.Name))
                {
                    dbContext.RoleClaims.Add(new ApplicationRoleClaim
                    {
                        RoleId = role.Id,
                        ClaimType = AClaims.Permission,
                        ClaimValue = permission.Name,
                        CreatedBy = "ApplicationDbSeeder"
                    });
                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            }
        }

        private static async Task<ApplicationUser> SeedSuperAdminUserAsync(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext dbContext,
            CancellationToken cancellationToken)
        {
            if (await userManager.Users.FirstOrDefaultAsync(u => u.Email == BusinessSuperAdmin.Email, cancellationToken: cancellationToken) is not ApplicationUser superAdminUser)
            {
                superAdminUser = new ApplicationUser
                {
                    FirstName = BusinessSuperAdmin.FirstName,
                    LastName = BusinessSuperAdmin.LastName,
                    Email = BusinessSuperAdmin.Email,
                    UserName = BusinessSuperAdmin.UserName,
                    EmailConfirmed = true,
                    PhoneNumber = BusinessSuperAdmin.PhoneNumber,
                    NormalizedEmail = BusinessSuperAdmin.Email.ToUpperInvariant(),
                    NormalizedUserName = BusinessSuperAdmin.UserName.ToUpperInvariant(),
                    IsActive = true
                };

                var password = new PasswordHasher<ApplicationUser>();
                superAdminUser.PasswordHash = password.HashPassword(superAdminUser, BusinessSuperAdmin.Password);
                await userManager.CreateAsync(superAdminUser);
                await userManager.UpdateAsync(superAdminUser);
                await dbContext.SaveChangesAsync(cancellationToken);
            }

            return superAdminUser;
        }

    }
}
