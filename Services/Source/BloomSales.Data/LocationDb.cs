using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using BloomSales.Data.Entities;

namespace BloomSales.Data
{
    internal class LocationDb : DbContext
    {
        public LocationDb() : base("name = LocationDatabase")
        {
            // do nothing!
        }

        public virtual DbSet<Region> Regions { get; set; }

        public virtual DbSet<Province> Provinces { get; set; }

        public virtual DbSet<Warehouse> Warehouses { get; set; }
    }
}
