using Microsoft.AspNetCore.Http;

namespace Academy.Application.Identity.Users
{
    public class UpdateUserRequest
    {
        public DefaultIdType Id { get; set; } = default!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public bool DeleteCurrentImage { get; set; } = false;
        public IFormFile? Image { get; set; }
    }
}