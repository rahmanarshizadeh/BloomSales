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
        [OperationContract]
        IEnumerable<Shipper> GetAllShippers();

        [OperationContract]
        IEnumerable<DeliveryService> GetServicesByShipper(string shipper);

        [OperationContract]
        void RequestShipping(ShippingInfo shipping);

        [OperationContract]
        ShippingStatus GetShippingStatus(int orderID);

        [OperationContract]
        ShippingInfo GetShipping(int orderID);

        [OperationContract]
        void AddShipper(Shipper shipper);

        [OperationContract]
        void AddDeliveryService(DeliveryService service);
    }
}