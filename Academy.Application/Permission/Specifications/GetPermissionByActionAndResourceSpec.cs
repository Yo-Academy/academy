using Academy.Domain.Entities;
using Ardalis.Specification;

namespace Academy.Application.CommonLookups.Specifications;

public class GetPermissionByActionAndResourceSpec : Specification<Permissions>, ISingleResultSpecification<Permissions>
{
    public GetPermissionByActionAndResourceSpec(string action, string resource, DefaultIdType? id = default)
    {
        Query.Where(x => x.Action.ToLower() == action.ToLower() && x.Resource.ToLower() == resource.ToLower() && (!id.HasValue || x.Id != id));
    }
}
