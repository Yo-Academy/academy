using Academy.Application.Permission.Dto;

namespace Academy.Application.Permission.Query.Models
{
    public class GetAllPermissionRequest : IRequest<Result<List<PermissionListDto>>>
    {
    }
}
