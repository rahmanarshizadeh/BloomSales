using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BloomSales.Data.Entities
{
    [DataContract]
    public class Warehouse : ContactInfo
    {
        [DataMember]
        public int ID { get; set; }

        [Required]
        [DataMember]
        public int RegionID { get; set; }

        [DataMember]
        public virtual Region Region { get; set; }
    }
}