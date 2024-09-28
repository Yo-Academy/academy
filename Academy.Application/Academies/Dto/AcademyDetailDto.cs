namespace Academy.Application.Academies.Dto
{
    public class AcademyDetailDto
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string AcademyId { get; set; }
        public string? GST { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }
        public string? QRCode { get; set; }
        public string? Logo { get; set; }
        public string? Subdomain { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<DefaultIdType> Sports { get; set; }
    }
}
