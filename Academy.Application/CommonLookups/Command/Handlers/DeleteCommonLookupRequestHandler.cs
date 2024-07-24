using Academy.Application.Common.Exceptions;
using Academy.Application.CommonLookups.Command.Models;
using Academy.Application.CommonLookups.Dto;
using Academy.Application.Persistence.Repository;
using Academy.Domain.Entities;
using Mapster;

namespace Academy.Application.CommonLookups.Command.Handlers;

public class DeleteCommonLookupRequestHandler : IRequestHandler<DeleteCommonLookupRequest, Result<CommonLookupsDto>>
{
    private readonly IRepository<CommonLookup> _repository;
    public DeleteCommonLookupRequestHandler(IRepository<CommonLookup> repository)
    {
        _repository = repository;
    }
    public async Task<Result<CommonLookupsDto>> Handle(DeleteCommonLookupRequest request, CancellationToken cancellationToken)
    {
        var commonLookupToDelete = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (commonLookupToDelete == null)
            return Result.Fail(new NotFoundException("Common Lookup Not Found"));

        await _repository.DeleteAsync(commonLookupToDelete, cancellationToken);
        return Result.Succeed(commonLookupToDelete.Adapt<CommonLookupsDto>());
    }
}
