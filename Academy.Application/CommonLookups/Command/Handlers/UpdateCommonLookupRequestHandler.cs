using Academy.Application.Common.Exceptions;
using Academy.Application.CommonLookups.Command.Models;
using Academy.Application.CommonLookups.Dto;
using Academy.Application.Persistence.Repository;
using Academy.Domain.Entities;
using Mapster;

namespace Academy.Application.CommonLookups.Command.Handlers;

public class UpdateCommonLookupRequestHandler : IRequestHandler<UpdateCommonLookupRequest, Result<CommonLookupsDto>>
{
    private readonly IRepository<CommonLookup> _repository;
    public UpdateCommonLookupRequestHandler(IRepository<CommonLookup> repository)
    {
        _repository = repository;
    }
    public async Task<Result<CommonLookupsDto>> Handle(UpdateCommonLookupRequest request, CancellationToken cancellationToken)
    {
        var commonLookupToUpdate = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (commonLookupToUpdate == null)
            return Result.Fail(new NotFoundException("CommonLookup Not Found"));

        commonLookupToUpdate.Update(request.Category, request.Key, request.Description, request.DisplayOrder);

        await _repository.UpdateAsync(commonLookupToUpdate, cancellationToken);
        return Result.Succeed(commonLookupToUpdate.Adapt<CommonLookupsDto>());
    }
}
