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
    public interface IOrderService
    {
        [OperationContract]
        bool PlaceOrder(Order order, ShippingInfo shipping, PaymentInfo payment);

        [OperationContract]
        Order GetOrder(int id);

        [OperationContract]
        IEnumerable<Order> GetOrderHistoryByCustomer(int customerID, DateTime startDate, DateTime endDate);

        [OperationContract]
        void AddOrUpdateCart(int customerID, Order order);

        [OperationContract]
        Order GetCart(int customerID);
    }
}