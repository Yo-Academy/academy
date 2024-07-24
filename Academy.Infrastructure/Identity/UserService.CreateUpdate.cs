using Academy.Application.Common.Mailing;
using Academy.Application.Common.Storage.Models;
using Academy.Application.Features.Mailing.Dto;
using Academy.Application.Identity.Users;
using Academy.Domain.Mailing;
using Academy.Shared.Authorization;
using Microsoft.Identity.Web;
using SendGrid.Helpers.Mail;
using System.Security.Claims;

namespace Academy.Infrastructure.Identity
{
    internal partial class UserService
    {
        /// <summary>
        /// This is used when authenticating with AzureAd.
        /// The local user is retrieved using the objectidentifier claim present in the ClaimsPrincipal.
        /// If no such claim is found, an InternalServerException is thrown.
        /// If no user is found with that ObjectId, a new one is created and populated with the values from the ClaimsPrincipal.
        /// If a role claim is present in the principal, and the user is not yet in that roll, then the user is added to that role.
        /// </summary>
        public async Task<string> GetOrCreateFromPrincipalAsync(ClaimsPrincipal principal)
        {
            string? objectId = principal.GetObjectId();
            if (string.IsNullOrWhiteSpace(objectId))
            {
                throw new InternalServerException(DbRes.T("InvalidObjectIdMsg"));
            }

            var user = await _userManager.Users.Where(u => u.ObjectId == objectId).FirstOrDefaultAsync()
                ?? await CreateOrUpdateFromPrincipalAsync(principal);

            if (principal.FindFirstValue(ClaimTypes.Role) is string role &&
                await _roleManager.RoleExistsAsync(role) &&
                !await _userManager.IsInRoleAsync(user, role))
            {
                await _userManager.AddToRoleAsync(user, role);
            }

            return user.Id.ToString();
        }

        private async Task<ApplicationUser> CreateOrUpdateFromPrincipalAsync(ClaimsPrincipal principal)
        {
            string? email = principal.FindFirstValue(ClaimTypes.Upn);
            string? username = principal.GetDisplayName();
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(username))
            {
                throw new InternalServerException(string.Format(DbRes.T("UsernameEmailNotValidMsg")));
            }

            var user = await _userManager.FindByNameAsync(username);
            if (user is not null && !string.IsNullOrWhiteSpace(user.ObjectId))
            {
                throw new InternalServerException(string.Format(DbRes.T("UsernameAlreadyTakenWithParamsMsg"), username));
            }

            if (user is null)
            {
                user = await _userManager.FindByEmailAsync(email);
                if (user is not null && !string.IsNullOrWhiteSpace(user.ObjectId))
                {
                    throw new InternalServerException(string.Format(DbRes.T("EmailAlreadyTakenWithParamsMsg"), email));
                }
            }

            IdentityResult? result;
            if (user is not null)
            {
                user.ObjectId = principal.GetObjectId();
                result = await _userManager.UpdateAsync(user);

                await _events.PublishAsync(new ApplicationUserUpdatedEvent(user.Id));
            }
            else
            {
                user = new ApplicationUser
                {
                    ObjectId = principal.GetObjectId(),
                    FirstName = principal.FindFirstValue(ClaimTypes.GivenName),
                    LastName = principal.FindFirstValue(ClaimTypes.Surname),
                    Email = email,
                    NormalizedEmail = email.ToUpperInvariant(),
                    UserName = username,
                    NormalizedUserName = username.ToUpperInvariant(),
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    IsActive = true
                };
                result = await _userManager.CreateAsync(user);

                await _events.PublishAsync(new ApplicationUserCreatedEvent(user.Id));
            }

            if (!result.Succeeded)
            {
                throw new InternalServerException(DbRes.T("ValidationErrorsOccurredMsg"), result.GetErrors());
            }

            return user;
        }

        public async Task<Result<UserDetailsDto>> CreateAsync(CreateUserRequest request, string origin)
        {
            var user = new ApplicationUser
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                PhoneNumber = request.PhoneNumber,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                throw new InternalServerException(DbRes.T("ValidationErrorsOccurredMsg"), result.GetErrors());
            }

            await _userManager.AddToRoleAsync(user, Roles.Basic);

            var messages = new List<string> { string.Format(DbRes.T("UserRegisteredWithParamsMsg"), user.UserName) };

            if (_securitySettings.RequireConfirmedAccount && !string.IsNullOrEmpty(user.Email))
            {
                //send verification email
                string emailVerificationUri = await GetEmailVerificationUriAsync(user, origin);
                RegisterUserEmailDto emailModel = new RegisterUserEmailDto()
                {
                    Email = user.Email,
                    UserName = user.UserName,
                    Url = emailVerificationUri
                };
                var emailTemplate = await _emailTemplateRepository.GetEmailTemplateByCodeAsync("CNFEM");
                var body = _emailHelper.GenerateEmailTemplate(emailTemplate.Body, emailModel);
                MailRequest mailRequest = new MailRequest() { To = new List<string> { user.Email }, Subject = DbRes.T(emailTemplate.Subject), Body = body };
                await _events.PublishAsync(new MailSendEvent(mailRequest));
                messages.Add(string.Format(DbRes.T("VerifyAccountWithParamsMsg"), user.Email));
            }

            await _events.PublishAsync(new ApplicationUserCreatedEvent(user.Id));

            return Result.Succeed(user.Adapt<UserDetailsDto>());
        }

        public async Task<Result<UserDetailsDto>> UpdateAsync(UpdateUserRequest request, DefaultIdType userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            _ = user ?? throw new NotFoundException(DbRes.T("UserNotFoundMsg"));

            string currentImage = user.ImageUrl ?? string.Empty;
            if (request.Image != null)
            {
                var res = await _storage.UploadAsync(request.Image.OpenReadStream(), new UploadOptions() { FileName = request.Image.FileName });
                user.ImageUrl = res.Value.Name;
            }

            if (request.DeleteCurrentImage && !string.IsNullOrEmpty(currentImage))
                await _storage.DeleteAsync(currentImage);

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.PhoneNumber = request.PhoneNumber;
            string? phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (request.PhoneNumber != phoneNumber)
            {
                await _userManager.SetPhoneNumberAsync(user, request.PhoneNumber);
            }

            var result = await _userManager.UpdateAsync(user);

            await _signInManager.RefreshSignInAsync(user);

            await _events.PublishAsync(new ApplicationUserUpdatedEvent(user.Id));

            if (!result.Succeeded)
            {
                throw new InternalServerException(DbRes.T("UpdateProfileFailedMsg"), result.GetErrors());
            }
            return Result.Succeed(user.Adapt<UserDetailsDto>());
        }

        public async Task<string> DeleteAsync(DefaultIdType id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            _ = user ?? throw new NotFoundException(DbRes.T("UserNotFoundMsg"));

            await _userManager.DeleteAsync(user);
            var userClaims = await _db.UserClaims.Where(x => x.UserId == user.Id).ToListAsync();
            if (userClaims.Any())
            {
                _db.UserClaims.RemoveRange(userClaims);
                await _db.SaveChangesAsync();
            }

            var userRoles = await _db.UserRoles.Where(x => x.UserId == user.Id).ToListAsync();
            if (userRoles.Any())
            {
                _db.UserRoles.RemoveRange(userRoles);
                await _db.SaveChangesAsync();
            }

            return string.Format(DbRes.T("UserDeletedWithParamsMsg"), user.FirstName);
        }
    }
}
