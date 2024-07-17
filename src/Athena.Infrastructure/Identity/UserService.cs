using Ardalis.Specification.EntityFrameworkCore;
using Athena.Application.Business;
using Athena.Application.Common.Caching;
using Athena.Application.Common.Exceptions;
using Athena.Application.Common.FileStorage;
using Athena.Application.Common.Mailing;
using Athena.Application.Common.Models;
using Athena.Application.Common.Specification;
using Athena.Application.Identity.Users;
using Athena.Domain.Identity;
using Athena.Infrastructure.Auth;
using Athena.Infrastructure.Persistence.Context;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace Athena.Infrastructure.Identity
{
    internal partial class UserService : IUserService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ApplicationDbContext _db;
        private readonly IStringLocalizer _t;
        private readonly IJobService _jobService;
        private readonly IMailService _mailService;
        private readonly SecuritySettings _securitySettings;
        private readonly IEmailTemplateService _templateService;
        private readonly IFileStorageService _fileStorage;
        private readonly IEventPublisher _events;
        private readonly ICacheService _cache;
        private readonly ICacheKeyService _cacheKeys;
        private readonly IAthenaAdmin _athenaAdmin;
        /*private readonly ITenantInfo _currentTenant;*/

        public UserService(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            ApplicationDbContext db,
            IStringLocalizer<UserService> localizer,
            IJobService jobService,
            IMailService mailService,
            IEmailTemplateService templateService,
            IFileStorageService fileStorage,
            IEventPublisher events,
            ICacheService cache,
            ICacheKeyService cacheKeys,
            IAthenaAdmin athenaAdmin,
           /* ITenantInfo currentTenant,*/
            IOptions<SecuritySettings> securitySettings)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
            _t = localizer;
            _jobService = jobService;
            _mailService = mailService;
            _templateService = templateService;
            _fileStorage = fileStorage;
            _events = events;
            _cache = cache;
            _cacheKeys = cacheKeys;
            _athenaAdmin = athenaAdmin;
            /*_currentTenant = currentTenant;*/
            _securitySettings = securitySettings.Value;
        }

        public async Task<PaginationResponse<UserDetailsDto>> SearchAsync(UserListFilter filter, CancellationToken cancellationToken)
        {
            var spec = new EntitiesByPaginationFilterSpec<ApplicationUser>(filter);

            var users = await _userManager.Users
                .WithSpecification(spec)
                .ProjectToType<UserDetailsDto>()
                .ToListAsync(cancellationToken);
            int count = await _userManager.Users
                .CountAsync(cancellationToken);

            return new PaginationResponse<UserDetailsDto>(users, count, filter.PageNumber, filter.PageSize);
        }

        public async Task<bool> ExistsWithNameAsync(string name)
        {
            EnsureValidTenant();
            return await _userManager.FindByNameAsync(name) is not null;
        }

        public async Task<bool> ExistsWithEmailAsync(string email, string? exceptId = null)
        {
            EnsureValidTenant();
            return await _userManager.FindByEmailAsync(email.Normalize()) is ApplicationUser user && user.Id != exceptId;
        }

        public async Task<bool> ExistsWithPhoneNumberAsync(string phoneNumber, string? exceptId = null)
        {
            EnsureValidTenant();
            return await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber) is ApplicationUser user && user.Id != exceptId;
        }

        private void EnsureValidTenant()
        {
            /*if (string.IsNullOrWhiteSpace(_currentTenant?.Id))
            {
                throw new UnauthorizedException(_t["Invalid Tenant."]);
            }*/
        }

        public async Task<List<UserDetailsDto>> GetListAsync(CancellationToken cancellationToken) =>
            (await _userManager.Users
                    .AsNoTracking()
                    .ToListAsync(cancellationToken))
                .Adapt<List<UserDetailsDto>>();

        public Task<int> GetCountAsync(CancellationToken cancellationToken) =>
            _userManager.Users.AsNoTracking().CountAsync(cancellationToken);

        public async Task<UserDetailsDto> GetAsync(string userId, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users
                .AsNoTracking()
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync(cancellationToken);

            _ = user ?? throw new NotFoundException(_t["User Not Found."]);

            return user.Adapt<UserDetailsDto>();
        }
        
        public async Task<UserDetailsDto?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users
                .AsNoTracking()
                .Where(u => u.Email == email)
                .FirstOrDefaultAsync(cancellationToken);

            return user?.Adapt<UserDetailsDto>();
        }

        public async Task ToggleStatusAsync(ToggleUserStatusRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.Where(u => u.Id == request.UserId).FirstOrDefaultAsync(cancellationToken);

            _ = user ?? throw new NotFoundException(_t["User Not Found."]);

           /* bool isAdmin = await _userManager.IsInRoleAsync(user, ARoles.Admin);
            if (isAdmin)
            {
                throw new ConflictException(_t["Administrators Profile's Status cannot be toggled"]);
            }*/

            user.IsActive = request.ActivateUser;

            await _userManager.UpdateAsync(user);

            await _events.PublishAsync(new ApplicationUserUpdatedEvent(user.Id));
        }
    }
}
