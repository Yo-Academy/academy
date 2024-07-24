using Academy.Application.CommonLookups.Dto;

namespace Academy.Application.CommonLookups.Command.Models;

public class UpdateCommonLookupRequest : IRequest<Result<CommonLookupsDto>>
{
    public DefaultIdType Id { get; set; }
    public string Category { get; set; } = default!;
    public string Key { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int DisplayOrder { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
}
