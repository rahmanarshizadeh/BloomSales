using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BloomSales.Data.Entities;

namespace BloomSales.Data.Repositories
{
    public class DeliveryServiceRepository : IDeliveryServiceRepository
    {
        private ShippingDb db;

        public DeliveryServiceRepository()
        {
            this.db = new ShippingDb();
        }

        internal DeliveryServiceRepository(ShippingDb context)
        {
            this.db = context;
        }

        public void AddService(DeliveryService service)
        {
            this.db.Services.Add(service);
            this.db.SaveChanges();
        }

        public DeliveryService GetService(int id)
        {
            var result = db.Services.Find(id);

            return result;
        }

        public IEnumerable<DeliveryService> GetServicesByShipper(int shipperID)
        {
            var result = (from s in db.Services
                          where s.ShipperID == shipperID
                          select s).ToArray();

            return result;
        }

        public void Dispose()
        {
            if (this.db != null)
                db.Dispose();
        }
    }
}
