using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BloomSales.Data.Entities
{
    [DataContract]
    [KnownType(typeof(Warehouse)), KnownType(typeof(Shipper)), KnownType(typeof(ShippingInfo))]
    public class ContactInfo
    {
        [Required]
        [StringLength(15)]
        [DataMember]
        public string City { get; set; }

        [Required]
        [StringLength(15)]
        [DataMember]
        public string Country { get; set; }

        [StringLength(35)]
        [DataMember]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        [DataMember]
        public string Name { get; set; }

        [StringLength(24)]
        [DataMember]
        public string Phone { get; set; }

        [Required]
        [StringLength(10)]
        [DataMember]
        public string PostalCode { get; set; }

        [StringLength(15)]
        [DataMember]
        public string Province { get; set; }

        [Required]
        [StringLength(80)]
        [DataMember]
        public string StreetAddress { get; set; }
    }
}