using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BloomSales.Data.Entities
{
    [DataContract]
    public class SalesTaxInfo
    {
        [Required]
        [DataMember]
        public string Country { get; set; }

        [Required]
        [DataMember]
        public float Federal { get; set; }

        [Key]
        [DataMember]
        public int ID { get; set; }

        [Required]
        [DataMember]
        public string Province { get; set; }

        [DataMember]
        public float Provincial { get; set; }
    }
}