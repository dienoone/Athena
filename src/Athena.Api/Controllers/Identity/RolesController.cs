using Athena.Application.Identity.Roles;

namespace Athena.Api.Controllers.Identity
{
    /*public class RolesController : VersionNeutralApiAuthGroupController
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService) => _roleService = roleService;

        [HttpGet]
        [MustHavePermission(AAction.View, AResource.Roles)]
        [OpenApiOperation("Get a list of all roles.", "")]
        public Task<List<RoleDto>> GetListAsync(CancellationToken cancellationToken)
        {
            return _roleService.GetListAsync(cancellationToken);
        }

        [HttpGet("{id}")]
        [MustHavePermission(AAction.View, AResource.Roles)]
        [OpenApiOperation("Get role details.", "")]
        public Task<RoleDto> GetByIdAsync(string id)
        {
            return _roleService.GetByIdAsync(id);
        }

        [HttpGet("{id}/permissions")]
        [MustHavePermission(AAction.View, AResource.RoleClaims)]
        [OpenApiOperation("Get role details with its permissions.", "")]
        public Task<RoleDto> GetByIdWithPermissionsAsync(string id, CancellationToken cancellationToken)
        {
            return _roleService.GetByIdWithPermissionsAsync(id, cancellationToken);
        }

        [HttpPut("{id}/permissions")]
        [MustHavePermission(AAction.Update, AResource.RoleClaims)]
        [OpenApiOperation("Update a role's permissions.", "")]
        public async Task<ActionResult<string>> UpdatePermissionsAsync(string id, UpdateRolePermissionsRequest request, CancellationToken cancellationToken)
        {
            if (id != request.RoleId)
            {
                return BadRequest();
            }

            return Ok(await _roleService.UpdatePermissionsAsync(request, cancellationToken));
        }

        [HttpPost]
        [MustHavePermission(AAction.Create, AResource.Roles)]
        [OpenApiOperation("Create or update a role.", "")]
        public Task<string> RegisterRoleAsync(CreateOrUpdateRoleRequest request)
        {
            return _roleService.CreateOrUpdateAsync(request);
        }

        [HttpDelete("{id}")]
        [MustHavePermission(AAction.Delete, AResource.Roles)]
        [OpenApiOperation("Delete a role.", "")]
        public Task<string> DeleteAsync(string id)
        {
            return _roleService.DeleteAsync(id);
        }
    }*/
}
