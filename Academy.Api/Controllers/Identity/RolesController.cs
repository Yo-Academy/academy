using Academy.Application.Academies.Query.Models;
using Academy.Application.Common.Exceptions;
using Academy.Application.Identity.Roles;
using Academy.Application.Identity.Roles.Query.Models;
using Academy.Application.Identity.Users;
using Academy.Infrastructure.Middleware;
using FluentValidation;

namespace Academy.API.Controllers.Identity
{
    public class RolesController : VersionNeutralApiController
    {
        private readonly IRoleService _roleService;
        private readonly IValidator<CreateRoleRequest> _createValidator;
        private readonly IValidator<UpdateRoleRequest> _updateValidator;
        private readonly IMediator _mediator;


        public RolesController(IRoleService roleService,
            IValidator<CreateRoleRequest> createValidator,
            IValidator<UpdateRoleRequest> updateValidator,
            IMediator mediator)
        {
            _roleService = roleService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _mediator = mediator;
        }

        [HttpGet]
        [MustHavePermission(Shared.Authorization.Action.View, Resource.Roles)]
        [OpenApiOperation("Get a list of all roles.", "")]
        public async Task<ActionResult> GetListAsync(CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(new GetRolesListRequest()));
        }

        [HttpGet("{id}")]
        [MustHavePermission(Shared.Authorization.Action.View, Resource.Roles)]
        [OpenApiOperation("Get role details.", "")]
        public async Task<ActionResult> GetByIdAsync(DefaultIdType id)
        {
            return Ok(await _roleService.GetByIdAsync(id));
        }

        [HttpGet("{id}/permissions")]
        [MustHavePermission(Shared.Authorization.Action.View, Resource.RoleClaims)]
        [OpenApiOperation("Get role details with its permissions.", "")]
        public async Task<ActionResult> GetByIdWithPermissionsAsync(DefaultIdType id, CancellationToken cancellationToken)
        {
            return Ok(await _roleService.GetByIdWithPermissionsAsync(id, cancellationToken));
        }

        [HttpPut("{id}/permissions")]
        [MustHavePermission(Shared.Authorization.Action.Update, Resource.RoleClaims)]
        [OpenApiOperation("Update a role's permissions.", "")]
        public async Task<ActionResult> UpdatePermissionsAsync(DefaultIdType id, UpdateRolePermissionsRequest request, CancellationToken cancellationToken)
        {
            if (id != request.RoleId)
            {
                return BadRequest(Result.Fail(new BadRequestException(DbRes.T("BadRequest"))));
            }
            return Ok(await _roleService.UpdatePermissionsAsync(request, cancellationToken));
        }

        [HttpPost]
        [MustHavePermission(Shared.Authorization.Action.Create, Resource.Roles)]
        [OpenApiOperation("Creates a role.", "")]
        public async Task<ActionResult> CreateAsync(CreateRoleRequest request)
        {
            var validationResult = await _createValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(Result.Fail(validationResult.Errors.Select(x => new Error() { Message = x.ErrorMessage }).ToArray()));
            }
            return Ok(await _roleService.CreateAsync(request));
        }

        [HttpPut("{id}")]
        [MustHavePermission(Shared.Authorization.Action.Update, Resource.Roles)]
        [OpenApiOperation("Updates a role.", "")]
        public async Task<ActionResult> UpdateAsync(Guid id, UpdateRoleRequest request)
        {
            request.Id = id;
            var validationResult = await _updateValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(Result.Fail(validationResult.Errors.Select(x => new Error() { Message = x.ErrorMessage }).ToArray()));
            }
            return Ok(await _roleService.UpdateAsync(request));
        }

        [HttpDelete("{id}")]
        [MustHavePermission(Shared.Authorization.Action.Delete, Resource.Roles)]
        [OpenApiOperation("Delete a role.", "")]
        public async Task<ActionResult> DeleteAsync(DefaultIdType id)
        {
            await _roleService.DeleteAsync(id);
            return NoContent();
        }
    }
}