namespace Academy.Application.CommonLookups.Dto;

public class CommonLookupsDto
{
    public DefaultIdType Id { get; set; }
    public string Category { get; set; } = default!;
    public string Key { get; set; } = default!;
    public string? Description { get; set; }
    public int? DisplayOrder { get; set; }
    public List<CommonLookupTranslationDto> CommonLookupTranslations { get; set; }
}
