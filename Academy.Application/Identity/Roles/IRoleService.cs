using Academy.Application.Identity.Roles.Dto;

namespace Academy.Application.Identity.Roles
{
    public interface IRoleService : ITransientService
    {
        Task<Result<List<RoleDto>>> GetListAsync(CancellationToken cancellationToken);

        Task<int> GetCountAsync(CancellationToken cancellationToken);

        Task<bool> ExistsAsync(string roleName, DefaultIdType? excludeId = default);

        Task<Result<RoleDto>> GetByIdAsync(DefaultIdType id);

        Task<Result<RoleDto>> GetByIdWithPermissionsAsync(DefaultIdType roleId, CancellationToken cancellationToken);

        Task<Result<RoleDto>> CreateAsync(CreateRoleRequest request);

        Task<Result<RoleDto>> UpdateAsync(UpdateRoleRequest request);

        Task<Result<RoleDto>> UpdatePermissionsAsync(UpdateRolePermissionsRequest request, CancellationToken cancellationToken);

        Task<string> DeleteAsync(DefaultIdType id);
        Task<Guid> GetRoleByRoleCodeAsync(string roleName);
        Task<List<string>> GetPermissionsByRoleId(DefaultIdType roleId);
    }
}