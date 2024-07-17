using Athena.Application.Business;
using Athena.Infrastructure.Identity;
using Athena.Shared.Business;
using Microsoft.AspNetCore.Identity;

namespace Athena.Infrastructure.Persistence
{
    public class AthenaAdmin : IAthenaAdmin
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AthenaAdmin(UserManager<ApplicationUser> userManager) =>
            (_userManager) = (userManager);
        
        public async Task<Guid> GetAthenBusinessId()
        {
            var admin = await _userManager.FindByEmailAsync(BusinessConstants.Email);
            return admin.BusinessId;
        }
    }
}
