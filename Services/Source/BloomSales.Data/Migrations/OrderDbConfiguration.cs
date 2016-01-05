namespace BloomSales.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class OrderDbConfiguration : DbMigrationsConfiguration<BloomSales.Data.OrderDb>
    {
        public OrderDbConfiguration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BloomSales.Data.OrderDb context)
        {
            // nothing!
        }
    }
}