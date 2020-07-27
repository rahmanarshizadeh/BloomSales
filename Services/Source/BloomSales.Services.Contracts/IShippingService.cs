using BloomSales.Data.Entities;
using System.Collections.Generic;
using System.ServiceModel;

namespace BloomSales.Services.Contracts
{
    [ServiceContract]
    public interface IShippingService
    {
        [OperationContract]
        void AddDeliveryService(DeliveryService service);

        [OperationContract]
        void AddShipper(Shipper shipper);

        [OperationContract]
        IEnumerable<Shipper> GetAllShippers();

        [OperationContract]
        IEnumerable<DeliveryService> GetServicesByShipper(string shipper);

        [OperationContract]
        ShippingInfo GetShipping(int orderID);

        [OperationContract]
        ShippingStatus GetShippingStatus(int orderID);

        [OperationContract]
        void RequestShipping(ShippingInfo shipping);
    }
}