using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BloomSales.Data.Entities
{
    [DataContract]
    public class ShippingInfo : ContactInfo
    {
        [Key]
        [DataMember]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OrderID { get; set; }

        [Required]
        [DataMember]
        public int WarehouseID { get; set; }

        [NotMapped]
        [DataMember]
        public Warehouse PickupLocation { get; set; }

        public DateTime ShippedDate { get; set; }

        [Required]
        [DataMember]
        public ShippingStatus Status { get; set; }

        [Required]
        [DataMember]
        public int ServiceID { get; set; }

        [DataMember]
        public virtual DeliveryService Service { get; set; }
    }
}