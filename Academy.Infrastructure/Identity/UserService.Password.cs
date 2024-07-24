using Academy.Application.Features.Mailing.Dto;

using Academy.Application.Identity.Users.Password;
using Academy.Domain.Mailing;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace Academy.Infrastructure.Identity
{
    internal partial class UserService
    {
        public async Task<Result<string>> ForgotPasswordAsync(ForgotPasswordRequest request, string origin)
        {
            EnsureValidTenant();

            var user = await _userManager.FindByEmailAsync(request.Email.Normalize());
            if (user is null || !await _userManager.IsEmailConfirmedAsync(user))
            {
                // Don't reveal that the user does not exist or is not confirmed
                throw new InternalServerException(DbRes.T("ErrorOccurredMsg"));
            }

            // For more information on how to enable account confirmation and password reset please
            // visit https://go.microsoft.com/fwlink/?LinkID=532713
            string code = await _userManager.GeneratePasswordResetTokenAsync(user);
            const string route = "account/reset-password";
            var endpointUri = new Uri(string.Concat($"{origin}/", route));
            string passwordResetUrl = QueryHelpers.AddQueryString(endpointUri.ToString(), "Token", WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code)));
            RegisterUserEmailDto emailModel = new RegisterUserEmailDto()
            {
                Email = user.Email,
                UserName = user.UserName,
                Url = passwordResetUrl
            };
            var emailTemplate = await _emailTemplateRepository.GetEmailTemplateByCodeAsync("RSPW");
            var body = _emailHelper.GenerateEmailTemplate(emailTemplate.Body, emailModel);
            MailRequest mailRequest = new MailRequest() { To = new List<string> { user.Email }, Subject = DbRes.T(emailTemplate.Subject), Body = body };
            await _events.PublishAsync(new MailSendEvent(mailRequest));

            return Result.Succeed(DbRes.T("PasswordResetMailSentMsg"));
        }

        public async Task<Result<string>> ResetPasswordAsync(ResetPasswordRequest request)
        {
            EnsureValidTenant();
            var user = await _userManager.FindByEmailAsync(request.Email?.Normalize()!);

            // Don't reveal that the user does not exist
            _ = user ?? throw new InternalServerException(DbRes.T("ErrorOccurredMsg"));

            var result = await _userManager.ResetPasswordAsync(user, Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token!)), request.Password!);

            return result.Succeeded
                ? Result.Succeed(DbRes.T("PasswordResetMsg"))
                : throw new InternalServerException(DbRes.T("ErrorOccurredMsg"));
        }

        public async Task ChangePasswordAsync(ChangePasswordRequest model, DefaultIdType userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            _ = user ?? throw new NotFoundException(DbRes.T("UserNotFoundMsg"));

            var result = await _userManager.ChangePasswordAsync(user, model.Password, model.NewPassword);

            if (!result.Succeeded)
            {
                throw new InternalServerException(DbRes.T("ChangePasswordFailedMsg"), result.GetErrors());
            }
        }
    }
}