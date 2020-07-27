using BloomSales.Data.Entities;
using BloomSales.Services.Contracts;
using System.Collections.Generic;
using System.ServiceModel;

namespace BloomSales.Services.Proxies
{
    public class ShippingClient : ClientBase<IShippingService>, IShippingService
    {
        public void AddDeliveryService(DeliveryService service)
        {
            Channel.AddDeliveryService(service);
        }

        public void AddShipper(Shipper shipper)
        {
            Channel.AddShipper(shipper);
        }

        public IEnumerable<Shipper> GetAllShippers()
        {
            return Channel.GetAllShippers();
        }

        public IEnumerable<DeliveryService> GetServicesByShipper(string shipper)
        {
            return Channel.GetServicesByShipper(shipper);
        }

        public ShippingInfo GetShipping(int orderID)
        {
            return Channel.GetShipping(orderID);
        }

        public ShippingStatus GetShippingStatus(int orderID)
        {
            return Channel.GetShippingStatus(orderID);
        }

        public void RequestShipping(ShippingInfo shipping)
        {
            Channel.RequestShipping(shipping);
        }
    }
}