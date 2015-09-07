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
    public class Product
    {
        public int ID { get; set; }

        [Required]
        [StringLength(120)]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Producer { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }

        public bool IsDiscontinued { get; set; }

        [Required]
        public int CategoryID { get; set; }

        public virtual ProductCategory Category { get; set; }

        public string ImageUrl { get; set; }
    }
}
