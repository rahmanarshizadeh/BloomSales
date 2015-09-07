using BloomSales.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloomSales.Data
{
    internal class ShippingDb : DbContext
    {
        public ShippingDb() : base("name = ShippingDatabase")
        {
            // do nothing!
        }

        public virtual DbSet<Shipper> Shippers { get; set; }
        public virtual DbSet<DeliveryService> Services { get; set; }
        public virtual DbSet<ShippingInfo> Shippings { get; set; }
    }
}
