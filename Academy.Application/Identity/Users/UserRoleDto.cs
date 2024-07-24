namespace Academy.Application.Identity.Users
{
    public class UserRoleDto
    {
        public DefaultIdType? RoleId { get; set; }
        public string? RoleName { get; set; }
        public string? Description { get; set; }
        public bool Enabled { get; set; }
    }
}