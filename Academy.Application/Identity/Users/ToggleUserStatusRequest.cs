namespace Academy.Application.Identity.Users
{
    public class ToggleUserStatusRequest
    {
        public bool ActivateUser { get; set; }
        public DefaultIdType? UserId { get; set; }
    }
}
