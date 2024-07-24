using Academy.Application.CommonLookups.Dto;

namespace Academy.Application.CommonLookups.Query.Models;

public class GetCommonLookupsByTypeRequest : IRequest<Result<List<CommonLookupsDto>>>
{
    public string Type { get; set; }
}
