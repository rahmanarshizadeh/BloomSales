using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloomSales.Data.Entities
{
    public class Warehouse : ContactInfo
    {
        public int ID { get; set; }

        [Required]
        public int RegionID { get; set; }

        public virtual Region Region { get; set; }
    }
}
