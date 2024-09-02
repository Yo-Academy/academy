
using Academy.API.Controllers;
using Academy.Application.Identity.Roles;
using Academy.Application.Permission.Command.Models;
using Academy.Application.Permission.Query.Models;

namespace Academy.Api.Controllers.Permissions
{
    public class PermissionsController : VersionedApiController
    {
        private readonly IMediator _mediator;
        private readonly IRoleService _roleService;

        public PermissionsController(IMediator mediator, IRoleService roleService)
        {
            _mediator = mediator;
            _roleService = roleService;
        }

        [HttpGet]
        [OpenApiOperation("Gets a list of Permission.", "")]
        [ApiExplorerSettings(IgnoreApi = false)]
        public async Task<ActionResult> GetListAsync()
        {
            return Ok(await _mediator.Send(new GetPermissionListRequest()));
        }

        [HttpGet("all-permissions")]
        [OpenApiOperation("Gets all Permissions.", "")]
        public async Task<ActionResult> GetAllPermissionsAsync()
        {
            return Ok(await _mediator.Send(new GetAllPermissionRequest()));
        }

        [HttpPost]
        [OpenApiOperation("Creates a Permission.", "")]
        public async Task<ActionResult> CreateAsync(CreatePermissionRequest createPermissionRequest)
        {
            return Ok(await _mediator.Send(createPermissionRequest));
        }

        [HttpDelete("{id}")]
        [OpenApiOperation("Deletes a Permission.", "")]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            return Ok(await _mediator.Send(new DeletePermissionRequest { Id = id }));
        }


        [HttpPut("{id}")]
        [OpenApiOperation("Updates a Permission.", "")]
        public async Task<ActionResult> UpdateAsync(Guid id, UpdatePermissionRequest updatePermissionRequest)
        {
            updatePermissionRequest.Id = id;
            return Ok(await _mediator.Send(updatePermissionRequest));
        }

        [HttpGet("current-user")]
        [OpenApiOperation("Get current user permissions.", "")]
        public async Task<ActionResult> GetPermissionsAsync()
        {
            var roleName = User.FindFirst(ClaimTypes.Role)?.Value;
            if (String.IsNullOrWhiteSpace(roleName))
            {
                return NoContent();
            }
            var roleId = await _roleService.GetRoleByRoleCodeAsync(roleName);
            if (roleId == Guid.Empty)
            {
                return NoContent();
            }
            return Ok(await _roleService.GetPermissionsByRoleId(roleId));
        }
    }
}
