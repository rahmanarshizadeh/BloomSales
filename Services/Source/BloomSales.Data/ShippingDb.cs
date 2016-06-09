using BloomSales.Data.Entities;
using System.Data.Entity;

namespace BloomSales.Data
{
    internal class ShippingDb : DbContext
    {
        public ShippingDb() : base("name = ShippingDatabase")
        {
            Database.SetInitializer<ShippingDb>(new ShippingDbInitializer());
            this.Configuration.ProxyCreationEnabled = false;
        }

        public virtual DbSet<Shipper> Shippers { get; set; }
        public virtual DbSet<DeliveryService> Services { get; set; }
        public virtual DbSet<ShippingInfo> Shippings { get; set; }
    }
}