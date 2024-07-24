using Academy.Shared.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace Academy.Infrastructure.Auth.Permissions
{
    public class MustHavePermissionAttribute : AuthorizeAttribute
    {
        public MustHavePermissionAttribute(string action, string resource) =>
            Policy = Permission.NameFor(action, resource);
    }
}