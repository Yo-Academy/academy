using Academy.Application.Common.Exceptions;
using Academy.Application.CommonLookups.Dto;
using Academy.Application.CommonLookups.Query.Models;
using Academy.Application.CommonLookups.Specifications;
using Academy.Application.Persistence.Repository;
using Academy.Domain.Entities;
using Mapster;

namespace Academy.Application.CommonLookups.Query.Handlers;

public class GetCommonLookupsByTypeRequestHandler : IRequestHandler<GetCommonLookupsByTypeRequest, Result<List<CommonLookupsDto>>>
{
    private readonly IRepository<CommonLookup> _repository;

    public GetCommonLookupsByTypeRequestHandler(IRepository<CommonLookup> repository) => _repository = repository;

    public async Task<Result<List<CommonLookupsDto>>> Handle(GetCommonLookupsByTypeRequest request, CancellationToken cancellationToken)
    {
		var todayDate = new DateOnly(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day);
        var commonLookups = (await _repository.ListAsync(new GetCommonLookupsByTypeSpec(request.Type), cancellationToken));

        if (commonLookups.Count == 0)
            return Result.Fail(new NotFoundException(string.Format("CommonLookup with Type '{0}' Not Found", request.Type)));

        return Result.Succeed(commonLookups.Adapt<List<CommonLookupsDto>>());
    }
}
