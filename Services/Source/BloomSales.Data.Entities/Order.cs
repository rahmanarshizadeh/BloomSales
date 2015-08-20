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

        [Required]
        public virtual IEnumerable<OrderItem> Items { get; set; }
    }
}
