﻿using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BloomSales.Data.Entities
{
    [DataContract]
    public class OrderItem : IIdentifiable
    {
        [DataMember]
        public float Discount { get; set; }

        [DataMember]
        public int ID { get; set; }

        [Required]
        [DataMember]
        public int OrderID { get; set; }

        [Required]
        [DataMember]
        public int ProductID { get; set; }

        [Required]
        [DataMember]
        public short Quantity { get; set; }

        [Required]
        [DataMember]
        public decimal UnitPrice { get; set; }
    }
}