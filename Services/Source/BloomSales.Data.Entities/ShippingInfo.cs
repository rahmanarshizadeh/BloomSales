using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace BloomSales.Data.Entities
{
    [DataContract]
    public class ShippingInfo : ContactInfo
    {
        [Key]
        [DataMember]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OrderID { get; set; }

        [NotMapped]
        [DataMember]
        public Warehouse PickupLocation { get; set; }

        [DataMember]
        public virtual DeliveryService Service { get; set; }

        [Required]
        [DataMember]
        public int ServiceID { get; set; }

        public DateTime ShippedDate { get; set; }

        [Required]
        [DataMember]
        public ShippingStatus Status { get; set; }

        [Required]
        [DataMember]
        public int WarehouseID { get; set; }
    }
}