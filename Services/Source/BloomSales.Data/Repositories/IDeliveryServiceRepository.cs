using BloomSales.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloomSales.Data.Repositories
{
    public interface IDeliveryServiceRepository : IRepository
    {
        void AddService(DeliveryService service);

        DeliveryService GetService(int id);

        IEnumerable<DeliveryService> GetServicesByShipper(int shipperID);
    }
}
