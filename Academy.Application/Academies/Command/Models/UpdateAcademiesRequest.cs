using Academy.Application.Academies.Dto;
using Microsoft.AspNetCore.Http;

namespace Academy.Application.Academies.Command.Models
{
    public class UpdateAcademiesRequest : IRequest<Result<AcademiesDto>>
    {
        public DefaultIdType Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string GST { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }
        public bool IsActive { get; set; }
        public IFormFile? Logo { get; set; } = default!;
        public IFormFile? QR { get; set; } = default!;
        public List<DefaultIdType> Sports { get; set; }
    }
}
