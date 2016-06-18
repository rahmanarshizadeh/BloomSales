using BloomSales.Data.Entities;
using System;
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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Properties<DateTime>()
                .Configure(c => c.HasColumnType("datetime2"));
        }

        public virtual DbSet<Shipper> Shippers { get; set; }
        public virtual DbSet<DeliveryService> Services { get; set; }
        public virtual DbSet<ShippingInfo> Shippings { get; set; }
    }
}