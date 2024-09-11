using Academy.Application.Permission.Dto;

namespace Academy.Application.Permission.Command.Models
{
    public class UpdatePermissionRequest : IRequest<Result<PermissionDto>>
    {
        public DefaultIdType Id { get; set; }
        public string Action { get; set; } = default!;
        public string Resource { get; set; } = default!;
        public string? Description { get; set; }
        public bool IsBasic { get; set; }
        public bool IsRoot { get; set; }
    }
}
