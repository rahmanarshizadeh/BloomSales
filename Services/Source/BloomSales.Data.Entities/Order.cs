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
        public int ID { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public int CustomerID { get; set; }

        public int ParentOrderID { get; set; }

        /// <summary>
        /// Indicates wether this is an internal order in BloomSales
        /// (from one warehouse to another)
        /// </summary>
        [Required]
        public bool IsInternalOrder { get; set; }

        [Required]
        public virtual IEnumerable<OrderItem> Items { get; set; }
    }
}
