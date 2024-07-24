using Academy.Application.Identity.Users;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Academy.Infrastructure.Auth.Permissions
{
    internal class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IUserService _userService;

        public PermissionAuthorizationHandler(IUserService userService) =>
            _userService = userService;

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User?.GetUserId() is { } userId &&
                await _userService.HasPermissionAsync(DefaultIdType.Parse(userId), requirement.Permission))
            {
                context.Succeed(requirement);
            }
        }
    }
}