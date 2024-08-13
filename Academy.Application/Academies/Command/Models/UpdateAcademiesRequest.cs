using Academy.Application.Academies.Dto;

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
    }
}
