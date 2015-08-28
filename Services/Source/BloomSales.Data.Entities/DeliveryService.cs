using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BloomSales.Data.Entities
{
    [DataContract]
    public class DeliveryService
    {
        public int ID { get; set; }

        [Required]
        [StringLength(20)]
        public string ServiceName { get; set; }

        [Required]
        public decimal Cost { get; set; }

        [Required]
        public int ShipperID { get; set; }

        [Required]
        public virtual Shipper Shipper { get; set; }
    }
}
