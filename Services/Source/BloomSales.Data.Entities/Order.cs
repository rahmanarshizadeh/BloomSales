using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BloomSales.Data.Entities
{
    [DataContract]
    public class Order
    {
        [DataMember]
        public int ID { get; set; }

        [Required]
        [DataMember]
        public DateTime OrderDate { get; set; }

        [Required]
        [DataMember]
        public int CustomerID { get; set; }

        [DataMember]
        public int ParentOrderID { get; set; }

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
    }
}