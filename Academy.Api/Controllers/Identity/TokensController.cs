using Academy.Application.Identity.Tokens;
using Academy.Application.Identity.Users;
using Academy.Application.Identity.Users.Command.Model;
using Elasticsearch.Net;
using YamlDotNet.Core.Tokens;

namespace Academy.API.Controllers.Identity
{
    public sealed class TokensController : VersionNeutralApiController
    {
        private readonly ITokenService _tokenService;

        public TokensController(ITokenService tokenService) => _tokenService = tokenService;

        [HttpPost("login-token")]
        [AllowAnonymous]
        [TenantIdHeader]
        [OpenApiOperation("Request an access token using credentials.", "")]
        public Task<TokenResponse> GetLoginTokenAsync(LoginRequest request, CancellationToken cancellationToken)
        {
            return _tokenService.GetLoginTokenAsync(request, GetIpAddress()!, cancellationToken);
        }

        [HttpPost]
        [AllowAnonymous]
        [TenantIdHeader]
        [OpenApiOperation("Request an access token using credentials.", "")]
        public Task<TokenResponse> GetTokenAsync(TokenRequest request, CancellationToken cancellationToken)
        {
            return _tokenService.GetTokenAsync(request, GetIpAddress()!, cancellationToken);
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        [TenantIdHeader]
        [OpenApiOperation("Request an access token using a refresh token.", "")]
        [ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.Search))]
        public Task<TokenResponse> RefreshAsync(RefreshTokenRequest request)
        {
            return _tokenService.RefreshTokenAsync(request, GetIpAddress()!);
        }

        [HttpPost("get-otp")]
        [AllowAnonymous]
        [TenantIdHeader]
        [OpenApiOperation("Request an one time password.", "")]
        public async Task<ActionResult> GetOTP(UserLoginRequest request, CancellationToken cancellationToken)
        {
            return Ok(Result.Succeed(await _tokenService.GenerateOTP(request, cancellationToken)));
        }


        private string? GetIpAddress() =>
            Request.Headers.ContainsKey("X-Forwarded-For")
                ? Request.Headers["X-Forwarded-For"]
                : HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "N/A";
    }
}