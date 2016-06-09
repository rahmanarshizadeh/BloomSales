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
        [DataMember]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        [DataMember]
        public string ServiceName { get; set; }

        [Required]
        [DataMember]
        public decimal Cost { get; set; }

        [Required]
        [DataMember]
        public int ShipperID { get; set; }

        [Required]
        [DataMember]
        public virtual Shipper Shipper { get; set; }
    }
}