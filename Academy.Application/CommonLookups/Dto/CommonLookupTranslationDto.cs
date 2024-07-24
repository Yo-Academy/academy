using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Application.CommonLookups.Dto
{
    public class CommonLookupTranslationDto
    {
        public DefaultIdType Id { get; set; }
        public string LanguageCode { get; set; } = default!;
        public string Value { get; set; } = default!;
    }
}
