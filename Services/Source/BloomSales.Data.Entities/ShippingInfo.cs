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
        public int OrderID { get; set; }

        [Required]
        public int WarehouseID { get; set; }

        [NotMapped]
        public Warehouse PickupLocation { get; set; }

        public DateTime ShippedDate { get; set; }

        [Required]
        public ShippingStatus Status { get; set; }

        [Required]
        public int ServiceID { get; set; }

        public virtual DeliveryService Service { get; set; }
    }
}
