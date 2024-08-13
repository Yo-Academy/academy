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
        public string GST { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }
        public string QRCode { get; set; }
        public string Logo { get; set; }
        public Academies(DefaultIdType id, string name,string shortname, string academyId, string gst,
            string address, string city, string pincode)
        {
            Id = id;
            Name = name;
            ShortName = shortname;
            AcademyId = academyId;
            GST = gst;
            Address = address;
            City = city;
            Pincode = pincode;
        }

        public Academies Update(string name, string shortname, string gst,
            string address, string city, string pincode)
        {
            Name = name;
            ShortName = shortname;
            GST = gst;
            Address = address;
            City = city;
            Pincode = pincode;
            return this;
        }
    }
}
