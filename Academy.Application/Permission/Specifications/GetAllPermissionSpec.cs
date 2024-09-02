using Ardalis.Specification;
using Academy.Application.Permission.Query.Models;
using Academy.Shared.Authorization;
using Entity = Academy.Domain.Entities;

namespace Academy.Application.Permission.Specifications
{
    public class GetAllPermissionSpec : Specification<Entity.Permissions>, ISingleResultSpecification<Entity.Permissions>
    {
        public GetAllPermissionSpec(GetAllPermissionRequest request)
        {
            Query.OrderBy(x => x.Resource);
        }
    }
}
