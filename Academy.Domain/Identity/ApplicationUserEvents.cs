namespace Academy.Domain.Identity
{
    public abstract class ApplicationUserEvent : DomainEvent
    {
        public DefaultIdType UserId { get; set; } = default!;

        protected ApplicationUserEvent(DefaultIdType userId) => UserId = userId;
    }

    public class ApplicationUserCreatedEvent : ApplicationUserEvent
    {
        public ApplicationUserCreatedEvent(DefaultIdType userId)
            : base(userId)
        {
        }
    }

    public class ApplicationUserUpdatedEvent : ApplicationUserEvent
    {
        public bool RolesUpdated { get; set; }

        public ApplicationUserUpdatedEvent(DefaultIdType userId, bool rolesUpdated = false)
            : base(userId) =>
            RolesUpdated = rolesUpdated;
    }
}