namespace Academy.Application.Identity.Roles.Dto
{
    public class RoleDto
    {
        public DefaultIdType Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public List<string>? Permissions { get; set; }
    }
}