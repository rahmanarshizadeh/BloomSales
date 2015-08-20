using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloomSales.Data.Entities
{
    public class InventoryItem
    {
        public int ID { get; set; }

        [Required]
        public int ProductID { get; set; }

        public virtual Product Product { get; set; }

        [Required]
        public int WarehouseID { get; set; }

        public virtual Warehouse Warehouse { get; set; }

        [Required]
        public short UnitsInStock { get; set; }
    }
}
