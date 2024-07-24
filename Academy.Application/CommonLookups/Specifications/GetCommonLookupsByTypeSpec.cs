using Academy.Domain.Entities;
using Ardalis.Specification;

namespace Academy.Application.CommonLookups.Specifications;

public class GetCommonLookupsByTypeSpec : Specification<CommonLookup>, ISingleResultSpecification<CommonLookup>
{
    public GetCommonLookupsByTypeSpec(string type)
    {
        Query.Where(x =>
            x.Category.ToLower() == type.ToLower())
            .Include(x => x.CommonLookupTranslations)
            .OrderBy(x => x.DisplayOrder);
    }
}
