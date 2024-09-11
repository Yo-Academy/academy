namespace Academy.Application.Permission.Dto
{
    public class PermissionDto
    {
        public DefaultIdType Id {  get; set; }
        public string Action { get; set; }
        public string Resource { get; set; }
        public string? Description { get; set; }
        public bool IsBasic { get; set; }
        public bool IsRoot { get; set; }
    }
}
