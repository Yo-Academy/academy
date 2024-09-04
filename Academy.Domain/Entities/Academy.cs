using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Domain.Entities
{
    public class Academies : AuditableEntity, IAggregateRoot
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
        public Academies() { }
        public Academies(DefaultIdType id, string name, string shortname, string? gst,
            string address, string city, string pincode, string logo, string qrcode, string academyId, int cnt, int padcount = 3)
        {
            Id = id;
            Name = name;
            ShortName = shortname;
            AcademyId = NameFor(cnt, academyId, padcount);
            GST = gst;
            Address = address;
            City = city;
            Pincode = pincode;
            Logo = logo;
            QRCode = qrcode;
            IsActive = true;
        }

        public Academies Update(string name, string shortname, string gst,
            string address, string city, string pincode, string? subdomain = default)
        {
            Name = name;
            ShortName = shortname;
            GST = gst;
            Address = address;
            City = city;
            Pincode = pincode;
            Subdomain = subdomain;
            return this;
        }

        private static string NameFor(int cnt, string shorname, int pad) => $"{shorname}{cnt.ToString().PadLeft(pad, '0')}";
    }
}
