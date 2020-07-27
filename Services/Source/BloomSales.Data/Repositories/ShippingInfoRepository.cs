using BloomSales.Data.Entities;
using System.Linq;

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

        public void Dispose()
        {
            if (this.db != null)
                db.Dispose();
        }

        public ShippingInfo GetShipping(int orderID)
        {
            var record = (from shipping in db.Shippings
                          where shipping.OrderID == orderID
                          select shipping).SingleOrDefault();

            record.Service = (from service in db.Services.Include("Shipper")
                              where service.ID == record.ServiceID
                              select service).SingleOrDefault();

            return record;
        }

        public ShippingStatus GetShippingStatus(int orderID)
        {
            ShippingInfo shipping = db.Shippings.Find(orderID);

            return shipping.Status;
        }
    }
}