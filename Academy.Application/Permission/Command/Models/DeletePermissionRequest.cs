using Academy.Application.Permission.Dto;

namespace Academy.Application.Permission.Command.Models
{
    public class DeletePermissionRequest : IRequest<Result<PermissionDto>>
    {
        public DefaultIdType Id { get; set; }
    }
}
