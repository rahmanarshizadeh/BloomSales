using BloomSales.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BloomSales.Services.Contracts
{
    [ServiceContract]
    public interface IShippingService
    {
        IEnumerable<Shipper> GetAllShippers();

        IEnumerable<DeliveryService> GetServicesByShipper(string shipper);

        void RequestShipping(ShippingInfo shipping);

        ShippingStatus GetShippingStatus(int orderID);
    }
}
