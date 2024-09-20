using Academy.Application.Common.Events;
using Academy.Application.Identity.Roles;
using Academy.Application.Identity.Roles.Dto;
using Academy.Application.Identity.Users;
using Academy.Infrastructure.Multitenancy;
using Academy.Infrastructure.Persistence.Context;
using Academy.Shared.Authorization;
using Academy.Shared.Multitenancy;
using Finbuckle.MultiTenant;
using Microsoft.Extensions.Configuration;

namespace Academy.Infrastructure.Identity
{
    internal class RoleService : IRoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;
        private readonly ICurrentUser _currentUser;
        private readonly ITenantInfo _currentTenant;
        private readonly IEventPublisher _events;
        private readonly IUserService _userService;
        private readonly IConfiguration _config;


        public RoleService(
            RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext db,
            ICurrentUser currentUser,
            ITenantInfo currentTenant,
            IEventPublisher events,
            IUserService userService,
            IConfiguration config)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _db = db;
            _currentUser = currentUser;
            _currentTenant = currentTenant;
            _events = events;
            _userService = userService;
            _config = config;
        }

        public async Task<Result<List<RoleDto>>> GetListAsync(CancellationToken cancellationToken) =>
            Result.Succeed((await _roleManager.Roles.ToListAsync(cancellationToken))
                .Adapt<List<RoleDto>>());

        public async Task<int> GetCountAsync(CancellationToken cancellationToken) =>
            await _roleManager.Roles.CountAsync(cancellationToken);

        public async Task<bool> ExistsAsync(string roleName, DefaultIdType? excludeId = default) =>
            await _roleManager.FindByNameAsync(roleName)
                is ApplicationRole existingRole
                && existingRole.Id != excludeId;

        public async Task<Result<RoleDto>> GetByIdAsync(DefaultIdType id) =>
            await _db.Roles.SingleOrDefaultAsync(x => x.Id == id) is { } role
                ? Result.Succeed(role.Adapt<RoleDto>())
                : throw new NotFoundException(DbRes.T("RoleNotFoundMsg"));

        public async Task<Result<RoleDto>> GetByIdWithPermissionsAsync(DefaultIdType roleId, CancellationToken cancellationToken)
        {
            var role = await GetByIdAsync(roleId);

            if (role.IsSuccess) role.Value.Permissions = await _db.RoleClaims
                                    .Where(c => c.RoleId == roleId && c.ClaimType == Claims.Permission)
                                    .Select(c => c.ClaimValue!)
                                    .ToListAsync(cancellationToken);

            return role;
        }

        public async Task<Result<RoleDto>> CreateAsync(CreateRoleRequest request)
        {
            // Create a new role.
            var role = new ApplicationRole(request.Name, request.Description);
            var result = await _roleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                throw new InternalServerException(DbRes.T("RegisterRoleFailedMsg"), result.GetErrors());
            }

            await _events.PublishAsync(new ApplicationRoleCreatedEvent(role.Id, role.Name!));

            return Result.Succeed(role.Adapt<RoleDto>());
        }

        public async Task<Result<RoleDto>> UpdateAsync(UpdateRoleRequest request)
        {
            // Update an existing role.
            var role = await _roleManager.FindByIdAsync(request.Id.ToString());

            _ = role ?? throw new NotFoundException(DbRes.T("RoleNotFoundMsg"));

            if (Roles.IsDefault(role.Name!))
            {
                throw new ConflictException(string.Format(DbRes.T("NotAllowedModifyRoleMsg"), role.Name));
            }

            role.Name = request.Name;
            role.NormalizedName = request.Name.ToUpperInvariant();
            role.Description = request.Description;
            var result = await _roleManager.UpdateAsync(role);

            if (!result.Succeeded)
            {
                throw new InternalServerException(DbRes.T("UpdateRoleFailedMsg"), result.GetErrors());
            }

            await _events.PublishAsync(new ApplicationRoleUpdatedEvent(role.Id, role.Name));

            return Result.Succeed(role.Adapt<RoleDto>());

        }

        public async Task<Result<RoleDto>> UpdatePermissionsAsync(UpdateRolePermissionsRequest request, CancellationToken cancellationToken)
        {
            var role = await _roleManager.FindByIdAsync(request.RoleId.ToString());
            _ = role ?? throw new NotFoundException(DbRes.T("RoleNotFoundMsg"));
            if (role.Name == Roles.Admin)
            {
                throw new ConflictException(DbRes.T("NotAllowedPermissionsRoleMsg"));
            }

            if (_currentTenant.Id != _config.GetSection(nameof(DefaultTenantSettings)).Get<DefaultTenantSettings>()!.Id)
            {
                // Remove Root Permissions if the Role is not created for Root Tenant.
                request.Permissions.RemoveAll(u => u.StartsWith("Permissions.Root."));
            }

            var currentClaims = await _roleManager.GetClaimsAsync(role);

            // Remove permissions that were previously selected
            foreach (var claim in currentClaims.Where(c => !request.Permissions.Any(p => p == c.Value)))
            {
                var removeResult = await _roleManager.RemoveClaimAsync(role, claim);
                if (!removeResult.Succeeded)
                {
                    throw new InternalServerException(DbRes.T("UpdatePermissionsFailedMsg"), removeResult.GetErrors());
                }
            }

            // Add all permissions that were not previously selected
            foreach (string permission in request.Permissions.Where(c => !currentClaims.Any(p => p.Value == c)))
            {
                if (!string.IsNullOrEmpty(permission))
                {
                    _db.RoleClaims.Add(new ApplicationRoleClaim
                    {
                        RoleId = role.Id,
                        ClaimType = Claims.Permission,
                        ClaimValue = permission,
                        CreatedBy = _currentUser.GetUserId()
                    });
                    await _db.SaveChangesAsync(cancellationToken);
                }
            }

            await _events.PublishAsync(new ApplicationRoleUpdatedEvent(role.Id, role.Name!, true));

            await _userService.InvalidatePermissionCacheAsync(_currentUser.GetUserId(), cancellationToken);

            var updatedrole = await GetByIdWithPermissionsAsync(role.Id, cancellationToken);
            return updatedrole;
        }

        public async Task<string> DeleteAsync(DefaultIdType id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());

            _ = role ?? throw new NotFoundException(DbRes.T("RoleNotFoundMsg"));

            if (Roles.IsDefault(role.Name!))
            {
                throw new ConflictException(string.Format(DbRes.T("NotAllowedDeleteRoleMsg"), role.Name));
            }

            if ((await _userManager.GetUsersInRoleAsync(role.Name!)).Count > 0)
            {
                throw new ConflictException(string.Format(DbRes.T("NotAllowedDeleteBeingUsedMsg"), role.Name));
            }

            await _roleManager.DeleteAsync(role);

            var roleClaims = await _db.RoleClaims.Where(x => x.RoleId == role.Id).ToListAsync();
            if (roleClaims.Any())
            {
                _db.RoleClaims.RemoveRange(roleClaims);
                await _db.SaveChangesAsync();
            }

            await _events.PublishAsync(new ApplicationRoleDeletedEvent(role.Id, role.Name!));

            return string.Format(DbRes.T("RoleDeletedWithParamsMsg"), role.Name);
        }

        public async Task<Guid> GetRoleByRoleCodeAsync(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                return Guid.Empty;
            }

            var role = await _db.Roles.FirstOrDefaultAsync(x => !String.IsNullOrWhiteSpace(x.Name) && x.Name.ToLower().Equals(roleName.ToLower()));

            return role?.Id ?? Guid.Empty;
        }

        public async Task<List<string>> GetPermissionsByRoleId(DefaultIdType roleId)
        {
            return await _db.RoleClaims.Where(x => x.ClaimType == Claims.Permission && x.RoleId == roleId).Select(x => x.ClaimValue).ToListAsync();
        }
    }
}