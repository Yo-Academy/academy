using Academy.Application.CommonLookups.Dto;

namespace Academy.Application.CommonLookups.Query.Models;

public class GetCommonLookupRequest : IRequest<Result<CommonLookupsDto>>
{
    public DefaultIdType Id { get; set; }
}
