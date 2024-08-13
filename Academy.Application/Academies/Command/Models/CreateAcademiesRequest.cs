using Academy.Application.Academies.Command.Validators;
using Academy.Application.Academies.Dto;

namespace Academy.Application.Academies.Command.Models
{
    public class CreateAcademiesRequest : IRequest<Result<AcademiesDto>>
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string AcademyId { get; set; }
        public string GST { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }
        public string QRCode { get; set; }
        public string Logo { get; set; }
    }
}
