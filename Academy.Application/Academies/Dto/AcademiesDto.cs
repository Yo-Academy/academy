using Academy.Application.Multitenancy;

namespace Academy.Application.Academies.Dto
{
    public class AcademiesDto
    {
        public string Name { get; set; }
        public string AcademyId { get; set; }
        public string GST { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }
        public string QRCode { get; set; }
        public DefaultIdType Id { get; set; }
    }

    public class AcademmyDetailsDto
    {
        public AcademiesDto Academy { get; set; }
        public TenantDto Tenant { get; set; }
    }
}
