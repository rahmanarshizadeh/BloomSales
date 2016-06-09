using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BloomSales.Data.Entities
{
    [DataContract]
    public class Province
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public int RegionID { get; set; }

        [DataMember]
        public virtual Region Region { get; set; }

        [Required]
        [DataMember]
        public string Name { get; set; }

        [Required]
        [StringLength(2)]
        [DataMember]
        public string Abbreviation { get; set; }
    }
}