using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BloomSales.Data.Entities
{
    [DataContract]
    public class DeliveryService : IIdentifiable
    {
        [Required]
        [DataMember]
        public decimal Cost { get; set; }

        [DataMember]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        [DataMember]
        public string ServiceName { get; set; }

        [Required]
        [DataMember]
        public virtual Shipper Shipper { get; set; }

        [Required]
        [DataMember]
        public int ShipperID { get; set; }
    }
}