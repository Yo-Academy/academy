using Academy.Application.Academies.Command.Models;
using Academy.Application.Common.Exceptions;
using Academy.Application.Identity.Tokens;
using Academy.Application.Identity.Users;
using Academy.Application.Identity.Users.Password;
using Academy.Infrastructure.Multitenancy;
using FluentValidation;

namespace Academy.API.Controllers.Identity
{
    public class UsersController : VersionNeutralApiController
    {
        private readonly IUserService _userService;
        private readonly IValidator<CreateUserRequest> _createValidator;
        private readonly IValidator<UpdateUserRequest> _updateValidator;
        private readonly IValidator<ForgotPasswordRequest> _forgotPasswordValidator;
        private readonly IValidator<ResetPasswordRequest> _resetPasswordValidator;
        private readonly Mediator _mediator;

        public UsersController(IUserService userService,
            IValidator<CreateUserRequest> createValidator,
            IValidator<UpdateUserRequest> updateValidator,
            IValidator<ForgotPasswordRequest> forgotPasswordValidator,
            IValidator<ResetPasswordRequest> resetPasswordValidator,
            Mediator mediator)
        {
            _userService = userService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _forgotPasswordValidator = forgotPasswordValidator;
            _resetPasswordValidator = resetPasswordValidator;
            _mediator = mediator;
        }

        [HttpGet]
        [MustHavePermission(Shared.Authorization.Action.View, Resource.Users)]
        [OpenApiOperation("Get list of all users.", "")]
        public async Task<ActionResult> GetListAsync(CancellationToken cancellationToken)
        {
            return Ok(await _userService.GetListAsync(cancellationToken));
        }

        [HttpGet("{id}")]
        [MustHavePermission(Shared.Authorization.Action.View, Resource.Users)]
        [OpenApiOperation("Get a user's details.", "")]
        public async Task<ActionResult> GetByIdAsync(DefaultIdType id, CancellationToken cancellationToken)
        {
            return Ok(await _userService.GetAsync(id, cancellationToken));
        }

        [HttpGet("{id}/roles")]
        [MustHavePermission(Shared.Authorization.Action.View, Resource.UserRoles)]
        [OpenApiOperation("Get a user's roles.", "")]
        public async Task<ActionResult> GetRolesAsync(DefaultIdType id, CancellationToken cancellationToken)
        {
            return Ok(await _userService.GetRolesAsync(id, cancellationToken));
        }

        [HttpPost("{id}/roles")]
        [ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.Register))]
        [MustHavePermission(Shared.Authorization.Action.Update, Resource.UserRoles)]
        [OpenApiOperation("Update a user's assigned roles.", "")]
        public async Task<ActionResult> AssignRolesAsync(DefaultIdType id, UserRolesRequest request, CancellationToken cancellationToken)
        {
            return Ok(await _userService.AssignRolesAsync(id, request, cancellationToken));
        }

        [HttpPost]
        [MustHavePermission(Shared.Authorization.Action.Create, Resource.Users)]
        [OpenApiOperation("Creates a new user.", "")]
        public async Task<ActionResult> CreateAsync(CreateUserRequest request)
        {
            // TODO: check if registering anonymous users is actually allowed (should probably be an appsetting)
            // and return UnAuthorized when it isn't
            // Also: add other protection to prevent automatic posting (captcha?)
            var validationResult = await _createValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(Result.Fail(validationResult.Errors.Select(x => new Error() { Message = x.ErrorMessage }).ToArray()));
            }
            Result<UserDetailsDto> user = await _userService.CreateAsync(request, GetOriginFromRequest());
            return CreatedAtRoute(new { id = user.Value?.Id }, user);
        }

        [HttpPost("self-register")]
        [TenantIdHeader]
        [AllowAnonymous]
        [OpenApiOperation("Anonymous user creates a user.", "")]
        [ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.Register))]
        public async Task<ActionResult> SelfRegisterAsync(CreateUserRequest request)
        {
            // TODO: check if registering anonymous users is actually allowed (should probably be an appsetting)
            // and return UnAuthorized when it isn't
            // Also: add other protection to prevent automatic posting (captcha?)
            var validationResult = await _createValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(Result.Fail(validationResult.Errors.Select(x => new Error() { Message = x.ErrorMessage }).ToArray()));
            }
            Result<UserDetailsDto> user = await _userService.CreateAsync(request, GetOriginFromRequest());
            return CreatedAtRoute(new { id = user.Value?.Id }, user);
        }

        [HttpPost("{id}/toggle-status")]
        [MustHavePermission(Shared.Authorization.Action.Update, Resource.Users)]
        [ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.Register))]
        [OpenApiOperation("Toggle a user's active status.", "")]
        public async Task<ActionResult> ToggleStatusAsync(DefaultIdType id, ToggleUserStatusRequest request, CancellationToken cancellationToken)
        {
            if (id != request.UserId)
            {
                return BadRequest(Result.Fail(new BadRequestException(DbRes.T("BadRequest"))));
            }

            return Ok(await _userService.ToggleStatusAsync(request, cancellationToken));
        }

        [HttpGet("confirm-email")]
        [AllowAnonymous]
        [OpenApiOperation("Confirm email address for a user.", "")]
        [ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.Search))]
        public async Task<ActionResult> ConfirmEmailAsync([FromQuery] string tenant, [FromQuery] DefaultIdType userId, [FromQuery] string code, CancellationToken cancellationToken)
        {
            return Ok(Result.Succeed(await _userService.ConfirmEmailAsync(userId, code, tenant, cancellationToken)));
        }

        [HttpGet("confirm-phone-number")]
        [AllowAnonymous]
        [OpenApiOperation("Confirm phone number for a user.", "")]
        [ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.Search))]
        public async Task<ActionResult> ConfirmPhoneNumberAsync([FromQuery] DefaultIdType userId, [FromQuery] string code)
        {
            return Ok(Result.Succeed(await _userService.ConfirmPhoneNumberAsync(userId, code)));
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        [TenantIdHeader]
        [OpenApiOperation("Request a password reset email for a user.", "")]
        [ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.Register))]
        public async Task<ActionResult> ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            var validationResult = await _forgotPasswordValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(Result.Fail(validationResult.Errors.Select(x => new Error() { Message = x.ErrorMessage }).ToArray()));
            }
            return Ok(await _userService.ForgotPasswordAsync(request, GetOriginFromRequest()));
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        [TenantIdHeader]
        [OpenApiOperation("Reset a user's password.", "")]
        [ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.Register))]
        public async Task<ActionResult> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var validationResult = await _resetPasswordValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(Result.Fail(validationResult.Errors.Select(x => new Error() { Message = x.ErrorMessage }).ToArray()));
            }
            return Ok(await _userService.ResetPasswordAsync(request));
        }

        private string GetOriginFromRequest() => $"{Request.Scheme}://{Request.Host.Value}{Request.PathBase.Value}";

        #region profile
        [HttpGet("profile")]
        [OpenApiOperation("Get profile details of currently logged in user.", "")]
        public async Task<ActionResult> GetProfileAsync(CancellationToken cancellationToken)
        {
            return User.GetUserId() is not { } userId || string.IsNullOrEmpty(userId)
                ? Unauthorized(Result.Fail(new UnauthorizedException(DbRes.T("AuthenticationFailedMsg"))))
                : Ok(await _userService.GetAsync(DefaultIdType.Parse(userId), cancellationToken));
        }

        [HttpPut("profile")]
        [OpenApiOperation("Update profile details of currently logged in user.", "")]
        public async Task<ActionResult> UpdateProfileAsync([FromForm] UpdateUserRequest request)
        {
            var validationResult = await _updateValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(Result.Fail(validationResult.Errors.Select(x => new Error() { Message = x.ErrorMessage }).ToArray()));
            }
            if (User.GetUserId() is not { } userId || string.IsNullOrEmpty(userId))
            {
                return Unauthorized(Result.Fail(new UnauthorizedException(DbRes.T("AuthenticationFailedMsg"))));
            }

            return Ok(await _userService.UpdateAsync(request, DefaultIdType.Parse(userId)));
        }

        [HttpPut("{id}")]
        [MustHavePermission(Shared.Authorization.Action.Update, Resource.Users)]
        [OpenApiOperation("Updates a user.", "")]
        public async Task<ActionResult> UpdateAsync(Guid id, [FromForm] UpdateUserRequest request)
        {
            request.Id = id;
            var validationResult = await _updateValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(Result.Fail(validationResult.Errors.Select(x => new Error() { Message = x.ErrorMessage }).ToArray()));
            }
            return Ok(await _userService.UpdateAsync(request, id));
        }

        [HttpPut("change-password")]
        [OpenApiOperation("Change password of currently logged in user.", "")]
        [ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.Register))]
        public async Task<ActionResult> ChangePasswordAsync(ChangePasswordRequest model)
        {
            if (User.GetUserId() is not { } userId || string.IsNullOrEmpty(userId))
            {
                return Unauthorized(Result.Fail(new UnauthorizedException(DbRes.T("AuthenticationFailedMsg"))));
            }

            await _userService.ChangePasswordAsync(model, DefaultIdType.Parse(userId));
            return Ok(Result.Succeed());
        }

        [HttpGet("permissions")]
        [OpenApiOperation("Get permissions of currently logged in user.", "")]
        public async Task<ActionResult> GetPermissionsAsync(CancellationToken cancellationToken)
        {
            return User.GetUserId() is not { } userId || string.IsNullOrEmpty(userId)
                ? Unauthorized(Result.Fail(new UnauthorizedException(DbRes.T("AuthenticationFailedMsg"))))
                : Ok(Result.Succeed(await _userService.GetPermissionsAsync(DefaultIdType.Parse(userId), cancellationToken)));
        }

        [HttpGet("logs")]
        [OpenApiOperation("Get audit logs of currently logged in user.", "")]
        public async Task<ActionResult> GetLogsAsync()
        {
            return Ok(await Mediator.Send(new GetMyAuditLogsRequest()));
        }

        [HttpDelete("{id}")]
        [MustHavePermission(Shared.Authorization.Action.Delete, Resource.Users)]
        [OpenApiOperation("Deletes a user.", "")]
        public async Task<ActionResult> DeleteAsync(DefaultIdType id)
        {
            await _userService.DeleteAsync(id);
            return NoContent();
        }
        #endregion

        [HttpGet("check-user/{phonenumber}")]
        [AllowAnonymous]
        [TenantIdHeader]
        [OpenApiOperation("Request an access token using credentials.", "")]
        public async Task<ActionResult> CheckUserAsync(string phonenumber, CancellationToken cancellationToken)
        {
            return Ok(Result.Succeed(await _userService.ExistsWithPhoneNumberAsync(phonenumber)));
        }

        //[HttpPost("academy-user")]
        //[OpenApiOperation("Creates an academy user.", "")]
        //[TenantIdHeader]
        //public async Task<ActionResult> CreateAcademyUserByRoleAsync(CreateAcademyUserRequest createAcademyUserCommand)
        //{
        //    createAcademyUserCommand.Origin = Request.Scheme;
        //    // Switch to a new tenant
        //    await _tenantResolverService.SwitchTenantAsync(HttpContext, createAcademyUserCommand.TenantId);

        //    Result<UserDetailsDto> createResult = await _mediator.Send(createAcademyUserCommand);

        //    await _tenantResolverService.RevertToPreviousTenantAsync(HttpContext);

        //    return CreatedAtRoute(new { id = createResult.Value?.Id }, createResult);
        //}
    }
}
