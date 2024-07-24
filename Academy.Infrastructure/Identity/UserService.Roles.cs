using Academy.Application.Identity.Users;
using Academy.Infrastructure.Multitenancy;
using Academy.Shared.Authorization;
using Academy.Shared.Multitenancy;
using Microsoft.Extensions.Configuration;

namespace Academy.Infrastructure.Identity
{
    internal partial class UserService
    {
        public async Task<Result<List<UserRoleDto>>> GetRolesAsync(DefaultIdType userId, CancellationToken cancellationToken)
        {
            var userRoles = new List<UserRoleDto>();

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null) throw new NotFoundException(DbRes.T("UserNotFoundMsg"));
            var roles = await _roleManager.Roles.AsNoTracking().ToListAsync(cancellationToken);
            if (roles is null) throw new NotFoundException(DbRes.T("RoleNotFoundMsg"));
            foreach (var role in roles)
            {
                userRoles.Add(new UserRoleDto
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    Description = role.Description,
                    Enabled = await _userManager.IsInRoleAsync(user, role.Name!)
                });
            }

            return Result.Succeed(userRoles);
        }

        public async Task<Result<string>> AssignRolesAsync(DefaultIdType userId, UserRolesRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            var user = await _userManager.Users.Where(u => u.Id == userId).FirstOrDefaultAsync(cancellationToken);

            _ = user ?? throw new NotFoundException(DbRes.T("UserNotFoundMsg"));

            // Check if the user is an admin for which the admin role is getting disabled
            if (await _userManager.IsInRoleAsync(user, Roles.Admin)
                && request.UserRoles.Any(a => !a.Enabled && a.RoleName == Roles.Admin))
            {
                // Get count of users in Admin Role
                int adminCount = (await _userManager.GetUsersInRoleAsync(Roles.Admin)).Count;
                var defaultTenantSettings = _config.GetSection(nameof(DefaultTenantSettings)).Get<DefaultTenantSettings>()!;

                // Check if user is not Root Tenant Admin
                // Edge Case : there are chances for other tenants to have users with the same email as that of Root Tenant Admin. Probably can add a check while User Registration
                if (user.Email == defaultTenantSettings.EmailAddress)
                {
                    if (_currentTenant.Id == defaultTenantSettings.Id)
                    {
                        throw new ConflictException(DbRes.T("NotRemoveAdminRoleRootTenantMsg"));
                    }
                }
                else if (adminCount <= 2)
                {
                    throw new ConflictException(DbRes.T("TenantMust2AdminMsg"));
                }
            }

            foreach (var userRole in request.UserRoles)
            {
                // Check if Role Exists
                if (await _roleManager.FindByNameAsync(userRole.RoleName!) is not null)
                {
                    if (userRole.Enabled)
                    {
                        if (!await _userManager.IsInRoleAsync(user, userRole.RoleName!))
                        {
                            await _userManager.AddToRoleAsync(user, userRole.RoleName!);
                        }
                    }
                    else
                    {
                        await _userManager.RemoveFromRoleAsync(user, userRole.RoleName!);
                    }
                }
            }

            await _events.PublishAsync(new ApplicationUserUpdatedEvent(user.Id, true));

            await InvalidatePermissionCacheAsync(user.Id, cancellationToken);

            return Result.Succeed(DbRes.T("UserRolesUpdatedMsg"));
        }
    }
}