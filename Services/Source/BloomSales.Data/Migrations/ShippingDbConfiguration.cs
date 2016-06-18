namespace BloomSales.Data.Migrations
{
    using BloomSales.Data.Entities;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class ShippingDbConfiguration : DbMigrationsConfiguration<ShippingDb>
    {
        public ShippingDbConfiguration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ShippingDb context)
        {
            // do nothing!
        }
    }
}