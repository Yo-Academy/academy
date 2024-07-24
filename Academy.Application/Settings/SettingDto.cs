using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Application.Settings
{
    public class SettingDto
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public string Key { get; set; } = default!;
        public string Value { get; set; } = default!;
        public string? Type { get; set; }
        public int? DisplayOrder { get; set; }
        public Guid Id { get; set; }
    }
}
