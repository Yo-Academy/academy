using Academy.Infrastructure.Common;
using Academy.Shared.Multitenancy;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace Academy.Infrastructure.Identity
{
    internal partial class UserService
    {
        private async Task<string> GetEmailVerificationUriAsync(ApplicationUser user, string origin)
        {
            EnsureValidTenant();

            string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            const string route = "api/users/confirm-email/";
            var endpointUri = new Uri(string.Concat($"{origin}/", route));
            string verificationUri = QueryHelpers.AddQueryString(endpointUri.ToString(), QueryStringKeys.UserId, user.Id.ToString());
            verificationUri = QueryHelpers.AddQueryString(verificationUri, QueryStringKeys.Code, code);
            verificationUri = QueryHelpers.AddQueryString(verificationUri, MultitenancyConstants.TenantIdName, _currentTenant.Id!);
            return verificationUri;
        }

        public async Task<string> ConfirmEmailAsync(DefaultIdType userId, string code, string tenant, CancellationToken cancellationToken)
        {
            EnsureValidTenant();

            var user = await _userManager.Users
                .Where(u => u.Id == userId && !u.EmailConfirmed)
                .FirstOrDefaultAsync(cancellationToken);

            _ = user ?? throw new InternalServerException(DbRes.T("ConfirmingEmailErrorMsg"));

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);

            return result.Succeeded
                ? string.Format(DbRes.T("AccountEmailConfirmedWithParamsMsg"), user.Email)
                : throw new InternalServerException(string.Format(DbRes.T("ConfirmingEmailErrorWithParamsMsg"), user.Email));
        }

        public async Task<string> ConfirmPhoneNumberAsync(DefaultIdType userId, string code)
        {
            EnsureValidTenant();

            var user = await _userManager.FindByIdAsync(userId.ToString());

            _ = user ?? throw new InternalServerException(DbRes.T("ConfirmingMobilePhoneErrorMsg"));
            if (string.IsNullOrEmpty(user.PhoneNumber)) throw new InternalServerException(DbRes.T("ConfirmingMobilePhoneErrorMsg"));

            var result = await _userManager.ChangePhoneNumberAsync(user, user.PhoneNumber, code);

            return result.Succeeded
                ? user.PhoneNumberConfirmed
                    ? string.Format(DbRes.T("AccountPhoneConfirmedWithParamsMsg"), user.PhoneNumber)
                    : string.Format(DbRes.T("AccountPhoneConfirmedWithParamsAlsoConfirmEmailMsg"), user.PhoneNumber)
                : throw new InternalServerException(string.Format(DbRes.T("ConfirmingPhoneErrorWithParamsMsg"), user.PhoneNumber));
        }
    }
}
