namespace Academy.Domain.Identity
{
    public abstract class ApplicationRoleEvent : DomainEvent
    {
        public DefaultIdType RoleId { get; set; } = default!;
        public string RoleName { get; set; } = default!;
        protected ApplicationRoleEvent(DefaultIdType roleId, string roleName) =>
            (RoleId, RoleName) = (roleId, roleName);
    }

    public class ApplicationRoleCreatedEvent : ApplicationRoleEvent
    {
        public ApplicationRoleCreatedEvent(DefaultIdType roleId, string roleName)
            : base(roleId, roleName)
        {
        }
    }

    public class ApplicationRoleUpdatedEvent : ApplicationRoleEvent
    {
        public bool PermissionsUpdated { get; set; }

        public ApplicationRoleUpdatedEvent(DefaultIdType roleId, string roleName, bool permissionsUpdated = false)
            : base(roleId, roleName) =>
            PermissionsUpdated = permissionsUpdated;
    }

    public class ApplicationRoleDeletedEvent : ApplicationRoleEvent
    {
        public bool PermissionsUpdated { get; set; }

        public ApplicationRoleDeletedEvent(DefaultIdType roleId, string roleName)
            : base(roleId, roleName)
        {
        }
    }
}