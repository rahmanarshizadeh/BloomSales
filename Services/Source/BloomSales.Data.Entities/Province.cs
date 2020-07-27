using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BloomSales.Data.Entities
{
    [DataContract]
    public class Province
    {
        [Required]
        [StringLength(2)]
        [DataMember]
        public string Abbreviation { get; set; }

        [DataMember]
        public int ID { get; set; }

        [Required]
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public virtual Region Region { get; set; }

        [DataMember]
        public int RegionID { get; set; }
    }
}