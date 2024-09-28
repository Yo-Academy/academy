using Academy.Application.Identity.Users;

namespace Academy.Application.Academies.Contracts
{
    public interface IAcademyUsersService //: ITransientService
    {
        Task<Result<List<UserDetailsDto>>> GetUsersAsync(string tenantId, CancellationToken cancellationToken);
    }
}