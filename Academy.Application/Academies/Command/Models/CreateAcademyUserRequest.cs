using Academy.Application.Academies.Dto;
using Academy.Application.Identity.Users;
using Academy.Shared.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Academy.Application.Academies.Command.Models
{
    public class CreateAcademyUserRequest : IRequest<Result>
    {
        public string TenantId { get; set; }
        public string FullName { get; set; } = default!;
        public string? PhoneNumber { get; set; }
        public string Role { get; set; }
        [JsonIgnore]
        public string Origin { get; set; } = default!;
    }

    public class AcademyUser
    {
        public string FullName { get; set; } = default!;
        public string? PhoneNumber { get; set; }
        public string Role { get; set; }
    }
}
