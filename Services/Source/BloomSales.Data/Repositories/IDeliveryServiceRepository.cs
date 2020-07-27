using BloomSales.Data.Entities;
using System.Collections.Generic;

namespace BloomSales.Data.Repositories
{
    public interface IDeliveryServiceRepository : IRepository
    {
        void AddService(DeliveryService service);

        DeliveryService GetService(int id);

        IEnumerable<DeliveryService> GetServicesByShipper(int shipperID);
    }
}