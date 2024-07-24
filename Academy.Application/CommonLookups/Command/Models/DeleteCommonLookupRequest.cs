using Academy.Application.CommonLookups.Dto;

namespace Academy.Application.CommonLookups.Command.Models;

public class DeleteCommonLookupRequest : IRequest<Result<CommonLookupsDto>>
{
    public Guid Id { get; set; }
}
