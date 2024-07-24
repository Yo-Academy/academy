using Academy.Application.CommonLookups.Command.Validators;
using Academy.Application.CommonLookups.Dto;

namespace Academy.Application.CommonLookups.Command.Models;

public class CreateCommonLookupRequest : IRequest<Result<CommonLookupsDto>>
{
    public string Category { get; set; } = default!;
    public string Key { get; set; } = default!;
    public string? Description { get; set; }
    public int? DisplayOrder { get; set; }
    public List<CommonLookUpTranslationModel> CommonLookUpTranslations { get; set; } = default!;
}
