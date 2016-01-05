
namespace BloomSales.Data.Migrations
{
    using BloomSales.Data.Entities;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class LocationDbConfiguration : DbMigrationsConfiguration<BloomSales.Data.LocationDb>
    {
        public LocationDbConfiguration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BloomSales.Data.LocationDb context)
        {
            // nothing!
        }
    }
}