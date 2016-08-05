using BloomSales.Data.Entities;
using BloomSales.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BloomSales.Services.Proxies
{
    public class ShippingClient : ClientBase<IShippingService>, IShippingService
    {
        public IEnumerable<Shipper> GetAllShippers()
        {
            return Channel.GetAllShippers();
        }

        public IEnumerable<DeliveryService> GetServicesByShipper(string shipper)
        {
            return Channel.GetServicesByShipper(shipper);
        }

        public void RequestShipping(ShippingInfo shipping)
        {
            Channel.RequestShipping(shipping);
        }

        public ShippingStatus GetShippingStatus(int orderID)
        {
            return Channel.GetShippingStatus(orderID);
        }

        public ShippingInfo GetShipping(int orderID)
        {
            return Channel.GetShipping(orderID);
        }

        public void AddShipper(Shipper shipper)
        {
            Channel.AddShipper(shipper);
        }

        public void AddDeliveryService(DeliveryService service)
        {
            Channel.AddDeliveryService(service);
        }
    }
}