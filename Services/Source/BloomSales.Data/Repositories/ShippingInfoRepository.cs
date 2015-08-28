using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BloomSales.Data.Entities;

namespace BloomSales.Data.Repositories
{
    public class ShippingInfoRepository : IShippingInfoRepository
    {
        private ShippingDb db;

        public ShippingInfoRepository()
        {
            this.db = new ShippingDb();
        }

        internal ShippingInfoRepository(ShippingDb context)
        {
            this.db = context;
        }

        public void AddShipping(ShippingInfo shipping)
        {
            db.Shippings.Add(shipping);
            db.SaveChanges();
        }

        public ShippingStatus GetShippingStatus(int orderID)
        {
            ShippingInfo shipping = db.Shippings.Find(orderID);

            return shipping.Status;
        }

        public void Dispose()
        {
            if (this.db != null)
                db.Dispose();
        }
    }
}
