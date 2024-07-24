using System.Linq;

namespace Academy.Domain.Entities
{
    public class CommonLookupTranslation : AuditableEntity, IAggregateRoot
    {
        public Guid CommonLookupId { get; set; }
        public string LanguageCode { get; set; } = default!;
        public string Value { get; set; } = default!;

        public CommonLookup CommonLookup { get; set; }

        public CommonLookupTranslation()
        {

        }
        public CommonLookupTranslation(DefaultIdType commonLookupId, string languageCode, string value)
        {
            CommonLookupId = commonLookupId;
            LanguageCode = languageCode;
            Value = value;
        }
    }
}
