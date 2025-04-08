using Academy.Application.Identity.Users.Command.Model;

namespace Academy.Application.Identity.Tokens
{
    public interface ITokenService : ITransientService
    {
        Task<TokenResponse> GetLoginTokenAsync(LoginRequest request, string ipAddress, CancellationToken cancellationToken);

        Task<TokenResponse> GetTokenAsync(TokenRequest request, string ipAddress, CancellationToken cancellationToken);

        Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request, string ipAddress);

        Task<bool> GenerateOTP(UserLoginRequest request, CancellationToken cancellationToken);
    }
}