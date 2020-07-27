using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BloomSales.Data.Entities
{
    [DataContract]
    public class Order : IIdentifiable
    {
        [Required]
        [DataMember]
        public string CustomerID { get; set; }

        [DataMember]
        public bool HasProcessed { get; set; }

        [DataMember]
        public int ID { get; set; }

        /// <summary>
        /// Indicates wether this is an internal order in BloomSales
        /// (from one warehouse to another)
        /// </summary>
        [Required]
        [DataMember]
        public bool IsInternalOrder { get; set; }

        [Required]
        [DataMember]
        public virtual IEnumerable<OrderItem> Items { get; set; }

        [Required]
        [DataMember]
        public DateTime OrderDate { get; set; }

        [DataMember]
        public int ParentOrderID { get; set; }

        [DataMember]
        public virtual IEnumerable<Order> SubOrders { get; set; }
    }
}