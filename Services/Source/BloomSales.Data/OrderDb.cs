using BloomSales.Data.Entities;
using System;
using System.Data.Entity;

namespace BloomSales.Data
{
    internal class OrderDb : DbContext
    {
        public OrderDb() : base("name = OrderDatabase")
        {
            Database.SetInitializer<OrderDb>(new OrderDbInitializer());
            this.Configuration.ProxyCreationEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Properties<DateTime>()
                .Configure(c => c.HasColumnType("datetime2"));
        }

        public virtual DbSet<Order> Orders { get; set; }

        public virtual DbSet<OrderItem> OrderItems { get; set; }
    }
}