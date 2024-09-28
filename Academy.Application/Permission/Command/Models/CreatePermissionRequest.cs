using Academy.Application.Permission.Dto;

namespace Academy.Application.Permission.Command.Models
{
    public class CreatePermissionRequest : IRequest<Result<PermissionDto>>
    {
        public string Action { get; set; } = default!;
        public string Resource { get; set; } = default!;
        public string? Description { get; set; }
        public bool IsBasic { get; set; }
        public bool IsRoot { get; set; }
    }
}
