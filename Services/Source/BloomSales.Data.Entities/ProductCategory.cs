using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloomSales.Data.Entities
{
    public class ProductCategory
    {
        [Key]
        public int ID { get; set; }

        public int? ParentID { get; set; }

        public virtual ProductCategory Parent { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
