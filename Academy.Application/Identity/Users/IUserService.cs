using Academy.Application.Academies.Command.Models;
using Academy.Application.Academies.Dto;
using Academy.Application.Identity.Users.Password;
using System.Security.Claims;

namespace Academy.Application.Identity.Users
{
    public interface IUserService : ITransientService
    {
        //Task<PaginationResponse<UserDetailsDto>> SearchAsync(UserListFilter filter, CancellationToken cancellationToken);

        Task<bool> ExistsWithNameAsync(string name);
        Task<bool> ExistsWithEmailAsync(string email, DefaultIdType? exceptId = null);
        Task<bool> ExistsWithPhoneNumberAsync(string phoneNumber, DefaultIdType? exceptId = null);

        Task<Result<List<UserDetailsDto>>> GetListAsync(CancellationToken cancellationToken);
        Task<List<AcademyUsersDetailsDto>> GetTenantUsersListAsync(CancellationToken cancellationToken);

        Task<int> GetCountAsync(CancellationToken cancellationToken);

        Task<Result<UserDetailsDto>> GetAsync(DefaultIdType userId, CancellationToken cancellationToken);

        Task<Result<List<UserRoleDto>>> GetRolesAsync(DefaultIdType userId, CancellationToken cancellationToken);
        Task<Result<string>> AssignRolesAsync(DefaultIdType userId, UserRolesRequest request, CancellationToken cancellationToken);

        Task<List<string>> GetPermissionsAsync(DefaultIdType userId, CancellationToken cancellationToken);
        Task<bool> HasPermissionAsync(DefaultIdType userId, string permission, CancellationToken cancellationToken = default);
        Task InvalidatePermissionCacheAsync(DefaultIdType userId, CancellationToken cancellationToken);

        Task<Result<UserDetailsDto>> ToggleStatusAsync(ToggleUserStatusRequest request, CancellationToken cancellationToken);

        Task<string> GetOrCreateFromPrincipalAsync(ClaimsPrincipal principal);
        Task<Result<UserDetailsDto>> CreateAsync(CreateUserRequest request, string origin);
        Task<Result<UserDetailsDto>> UpdateAsync(UpdateUserRequest request, DefaultIdType userId);

        Task<string> ConfirmEmailAsync(DefaultIdType userId, string code, string tenant, CancellationToken cancellationToken);
        Task<string> ConfirmPhoneNumberAsync(DefaultIdType userId, string code);

        Task<Result<string>> ForgotPasswordAsync(ForgotPasswordRequest request, string origin);
        Task<Result<string>> ResetPasswordAsync(ResetPasswordRequest request);
        Task ChangePasswordAsync(ChangePasswordRequest request, DefaultIdType userId);

        Task<string> DeleteAsync(DefaultIdType id);
        Task<Result<UserDetailsDto>> CreateAsyncWithTenantId(CreateAcademyUserRequest request, string origin);

        Task<List<AcademyUsersDetailsDto>> GetTenantUsersByRole(string role, CancellationToken cancellationToken);
    }
}