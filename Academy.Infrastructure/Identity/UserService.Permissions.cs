using Academy.Shared.Authorization;

namespace Academy.Infrastructure.Identity
{
    internal partial class UserService
    {
        public async Task<List<string>> GetPermissionsAsync(DefaultIdType userId, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            _ = user ?? throw new UnauthorizedException("Authentication Failed.");

            var userRoles = await _userManager.GetRolesAsync(user);
            var permissions = new List<string>();
            foreach (var role in await _roleManager.Roles
                .Where(r => userRoles.Contains(r.Name!))
                .ToListAsync(cancellationToken))
            {
                permissions.AddRange(await _db.RoleClaims
                    .Where(rc => rc.RoleId == role.Id && rc.ClaimType == Claims.Permission)
                    .Select(rc => rc.ClaimValue!)
                    .ToListAsync(cancellationToken));
            }

            return permissions.Distinct().ToList();
        }

        public async Task<bool> HasPermissionAsync(DefaultIdType userId, string permission, CancellationToken cancellationToken)
        {
            var permissions = await _cache.GetOrSetAsync(
                _cacheKeys.GetCacheKey(Claims.Permission, userId),
                () => GetPermissionsAsync(userId, cancellationToken),
                cancellationToken: cancellationToken);

            return permissions?.Contains(permission) ?? false;
        }

        public Task InvalidatePermissionCacheAsync(DefaultIdType userId, CancellationToken cancellationToken) =>
            _cache.RemoveAsync(_cacheKeys.GetCacheKey(Claims.Permission, userId), cancellationToken);
    }
}