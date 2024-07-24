using Academy.Application.CommonLookups.Command.Models;
using Academy.Application.CommonLookups.Dto;
using Academy.Application.Contracts.Persistence;
using Academy.Domain.Entities;
using Mapster;

namespace Academy.Application.CommonLookups.Command.Handlers;

public class CreateCommonLookupRequestHandler : IRequestHandler<CreateCommonLookupRequest, Result<CommonLookupsDto>>
{
    private readonly IRepository<CommonLookup> _repository;

    public CreateCommonLookupRequestHandler(IRepository<CommonLookup> repository)
    {
        _repository = repository;
    }

    public async Task<Result<CommonLookupsDto>> Handle(CreateCommonLookupRequest request, CancellationToken cancellationToken)
    {
        var id = Guid.NewGuid();
        CommonLookup commonLookup = new CommonLookup(id, request.Category, request.Key, request.Description, request.DisplayOrder);

        if (request.CommonLookUpTranslations != null && request.CommonLookUpTranslations.Count > 0)
        {
            commonLookup.CommonLookupTranslations = request.CommonLookUpTranslations.Select(x => new CommonLookupTranslation(id, x.LanguageCode, x.Value)).ToList();
        }

        //Inserts RequirementSet Record
        var responseCommonLookup = await _repository.AddAsync(commonLookup);

        return Result.Succeed(responseCommonLookup.Adapt<CommonLookupsDto>());
    }
}

