using Academy.Domain.Entities;
using Ardalis.Specification;

namespace Academy.Application.CommonLookups.Specifications;

public class GetCommonLookupByKeySpec : Specification<CommonLookup>, ISingleResultSpecification<CommonLookup>
{
    public GetCommonLookupByKeySpec(string key, string type, Guid? id = default)
    {
        Query.Where(x => x.Key.ToLower() == key.ToLower() && x.Category.ToLower() == type.ToLower() && (!id.HasValue || id != x.Id));
    }
}
