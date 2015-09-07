using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloomSales.Data.Entities
{
    public class Province
    {
        public int ID { get; set; }

        public int RegionID { get; set; }

        public virtual Region Region { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [StringLength(2)]
        public string Abbreviation { get; set; }
    }
}
