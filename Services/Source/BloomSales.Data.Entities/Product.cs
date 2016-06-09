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
        [DataMember]
        public int ID { get; set; }

        [Required]
        [StringLength(120)]
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Producer { get; set; }

        [Required]
        [DataMember]
        public decimal UnitPrice { get; set; }

        [DataMember]
        public bool IsDiscontinued { get; set; }

        [Required]
        [DataMember]
        public int CategoryID { get; set; }

        [DataMember]
        public virtual ProductCategory Category { get; set; }

        [DataMember]
        public string ImageUrl { get; set; }
    }
}