using BloomSales.Data.Entities;
using System.Data.Entity;

namespace BloomSales.Data
{
    internal class InventoryDb : DbContext
    {
        public InventoryDb() : base("name = InventoryDatabase")
        {
            Database.SetInitializer<InventoryDb>(new InventoryDbInitializer());
            this.Configuration.ProxyCreationEnabled = false;
        }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<ProductCategory> Categories { get; set; }

        public virtual DbSet<InventoryItem> Inventories { get; set; }
    }
}