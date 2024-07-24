using Academy.Application.CommonLookups.Dto;
using Academy.Application.CommonLookups.Query.Models;
using Academy.Application.CommonLookups.Specifications;
using Academy.Application.Persistence.Repository;
using Academy.Domain.Entities;
using Academy.Shared.Pagination;
using Academy.Shared.Pagination.Models;

namespace Academy.Application.CommonLookups.Query.Handlers;

public class GetCommonLookupListRequestHandler : IRequestHandler<GetCommonLookupListRequest, Result<PaginationResponse<CommonLookupsDto>>>
{
    private readonly IReadRepository<CommonLookup> _repository;
    public GetCommonLookupListRequestHandler(IReadRepository<CommonLookup> repository)
    {
        _repository = repository;
    }
    public async Task<Result<PaginationResponse<CommonLookupsDto>>> Handle(GetCommonLookupListRequest request, CancellationToken cancellationToken)
    {
        var spec = new GetCommonLookupListSpec(request);
        var data = await _repository.PaginatedListAsync(spec, request.PageNumber, request.PageSize, cancellationToken);

        if (data.Data != null && data.Data.Count > 0)
        {
            var commonLookupList = data.Data.Select(x => new CommonLookupsDto
            {
                Id = x.Id,
                Category = x.Category,
                Key = x.Key,
                DisplayOrder = x.DisplayOrder                
            }).ToList();
            data.Data = commonLookupList;
        }
        return Result.Succeed(data);
    }
}
