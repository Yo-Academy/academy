using Academy.Application.Common.Exceptions;
using Academy.Application.CommonLookups.Dto;
using Academy.Application.CommonLookups.Query.Models;
using Academy.Application.Persistence.Repository;
using Academy.Domain.Entities;
using Mapster;

namespace Academy.Application.CommonLookups.Query.Handlers;

public class GetCommonLookupRequestHandler : IRequestHandler<GetCommonLookupRequest, Result<CommonLookupsDto>>
{
    private readonly IRepository<CommonLookup> _repository;

    public GetCommonLookupRequestHandler(IRepository<CommonLookup> repository)
    {
        _repository = repository;
    }
    public async Task<Result<CommonLookupsDto>> Handle(GetCommonLookupRequest request, CancellationToken cancellationToken)
    {
        var commonLookup = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (commonLookup == null)
            return Result.Fail(new NotFoundException("Common Lookup not found."));
        var commonLookupDto = commonLookup.Adapt<CommonLookupsDto>();
        return Result.Succeed(commonLookupDto);
    }
}
