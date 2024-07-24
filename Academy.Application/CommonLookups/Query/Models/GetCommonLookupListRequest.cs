using Academy.Application.CommonLookups.Dto;
using Academy.Shared.Pagination.Models;

namespace Academy.Application.CommonLookups.Query.Models;

public class GetCommonLookupListRequest : PaginationFilter, IRequest<Result<PaginationResponse<CommonLookupsDto>>>
{

}
