using Academy.Application.Academies.Command.Models;
using Academy.Application.Features.Mailing.Dto;
using Academy.Application.Identity.Users;
using Academy.Domain.Mailing;
using Academy.Infrastructure.Multitenancy;
using Academy.Shared;
using Finbuckle.MultiTenant;
using TenantInfo = Finbuckle.MultiTenant.TenantInfo;

namespace Academy.Infrastructure.Identity
{
    internal partial class UserService
    {
        public async Task<Result<UserDetailsDto>> CreateAsyncWithTenantId(CreateAcademyUserRequest request, string origin)
        {
            //// Switch the tenant context
            //var tenant = await _tenantResolver.ResolveAsync(request.TenantId);
            //if (tenant == null)
            //{
            //    throw new Exception("Tenant not found");
            //}

            // Save current tenant context
            var currentTenant = _multiTenantContextAccessor.MultiTenantContext?.TenantInfo;

            // Get the target tenant
            var targetTenant = await _dbTenant.TenantInfo.FirstOrDefaultAsync(
                t => t.Identifier == request.TenantId);

            if (targetTenant == null)
            {
                throw new Exception("Tenant not found");
            }

            // Switch to the target tenant context
            _multiTenantContextAccessor.MultiTenantContext = new MultiTenantContext<TenantInfo>()
            {
                TenantInfo = targetTenant.Adapt<TenantInfo>()
            };

            var names = request.FullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var user = new ApplicationUser
            {
                FirstName = names[0],
                LastName = names[1],
                UserName = request.PhoneNumber,
                PhoneNumber = request.PhoneNumber,
                IsActive = true,
                TenantId = request.TenantId,
                OTP = "1234"
            };

            try
            {
                var password = new PasswordHasher<ApplicationUser>();
                string defaultPassword = Constants.DefaultPassword;
                user.PasswordHash = password.HashPassword(user, defaultPassword);
                var result = await _userManager.CreateAsync(user, Constants.DefaultPassword);
                if (!result.Succeeded)
                {
                    throw new InternalServerException(DbRes.T("ValidationErrorsOccurredMsg"), result.GetErrors());
                }

                await _userManager.AddToRoleAsync(user, request.Role);

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
                    await _events.PublishAsync(new ApplicationUserCreatedEvent(user.Id));
                }
            }
            finally
            {
                // Revert to the original tenant context
                _multiTenantContextAccessor.MultiTenantContext = new MultiTenantContext<TenantInfo>
                {
                    TenantInfo = currentTenant ?? _currentTenant
                };
            }
            return Result.Succeed(user.Adapt<UserDetailsDto>());
        }

        //public async Task<Result<IdentityResult>> CreateUserInTenantAsync(string tenantIdentifier, string phonenumber, string firstname, string lastname)
        //{
        //    // Switch the tenant context
        //    var tenant = await _tenantResolver.ResolveAsync(tenantIdentifier);
        //    if (tenant == null)
        //    {
        //        throw new Exception("Tenant not found");
        //    }

        //    if (tenant.HasResolvedTenant)
        //    {
        //        var user = new ApplicationUser
        //        {
        //            UserName = phonenumber,
        //            FirstName = firstname,
        //            LastName = lastname,
        //            PhoneNumber = phonenumber,
        //            TenantId = tenant.TenantInfo?.Id // Ensure the user is associated with the tenant
        //        };

        //        var result = await _userManager.CreateAsync(user);
        //        return Result.Succeed(result);
        //    }
        //    return Result.Fail(new BadRequestException(DbRes.T("TenantNotfound")));
        //}


        //public async Task<Result<UserDetailsDto>> UpdateAsync(UpdateUserRequest request, DefaultIdType userId)
        //{
        //    var user = await _userManager.FindByIdAsync(userId.ToString());

        //    _ = user ?? throw new NotFoundException(DbRes.T("UserNotFoundMsg"));

        //    string currentImage = user.ImageUrl ?? string.Empty;
        //    if (request.Image != null)
        //    {
        //        var res = await _storage.UploadAsync(request.Image.OpenReadStream(), new UploadOptions() { FileName = request.Image.FileName });
        //        user.ImageUrl = res.Value.Name;
        //    }

        //    if (request.DeleteCurrentImage && !string.IsNullOrEmpty(currentImage))
        //        await _storage.DeleteAsync(currentImage);

        //    user.FirstName = request.FirstName;
        //    user.LastName = request.LastName;
        //    user.PhoneNumber = request.PhoneNumber;
        //    string? phoneNumber = await _userManager.GetPhoneNumberAsync(user);
        //    if (request.PhoneNumber != phoneNumber)
        //    {
        //        await _userManager.SetPhoneNumberAsync(user, request.PhoneNumber);
        //    }

        //    var result = await _userManager.UpdateAsync(user);

        //    await _signInManager.RefreshSignInAsync(user);

        //    await _events.PublishAsync(new ApplicationUserUpdatedEvent(user.Id));

        //    if (!result.Succeeded)
        //    {
        //        throw new InternalServerException(DbRes.T("UpdateProfileFailedMsg"), result.GetErrors());
        //    }
        //    return Result.Succeed(user.Adapt<UserDetailsDto>());
        //}

        //public async Task<string> DeleteAsync(DefaultIdType id)
        //{
        //    var user = await _userManager.FindByIdAsync(id.ToString());

        //    _ = user ?? throw new NotFoundException(DbRes.T("UserNotFoundMsg"));

        //    await _userManager.DeleteAsync(user);
        //    var userClaims = await _db.UserClaims.Where(x => x.UserId == user.Id).ToListAsync();
        //    if (userClaims.Any())
        //    {
        //        _db.UserClaims.RemoveRange(userClaims);
        //        await _db.SaveChangesAsync();
        //    }

        //    var userRoles = await _db.UserRoles.Where(x => x.UserId == user.Id).ToListAsync();
        //    if (userRoles.Any())
        //    {
        //        _db.UserRoles.RemoveRange(userRoles);
        //        await _db.SaveChangesAsync();
        //    }

        //    return string.Format(DbRes.T("UserDeletedWithParamsMsg"), user.FirstName);
        //}
    }
}
