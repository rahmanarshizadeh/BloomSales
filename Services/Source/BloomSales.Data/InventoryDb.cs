using BloomSales.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloomSales.Data
{
    internal class InventoryDb : DbContext
    {
        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<ProductCategory> Categories { get; set; }

        public virtual DbSet<InventoryItem> Inventories { get; set; }
    }
}
