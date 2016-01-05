using BloomSales.Data.Entities;
using System.Data.Entity;

namespace BloomSales.Data
{
    internal class LocationDb : DbContext
    {
        public LocationDb() : base("name = LocationDatabase")
        {
            Database.SetInitializer<LocationDb>(new LocationDbInitializer());
        }

        public virtual DbSet<Region> Regions { get; set; }

        public virtual DbSet<Province> Provinces { get; set; }

        public virtual DbSet<Warehouse> Warehouses { get; set; }
    }
}