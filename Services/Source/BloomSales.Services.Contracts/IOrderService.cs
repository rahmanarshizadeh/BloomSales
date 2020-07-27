using BloomSales.Data.Entities;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace BloomSales.Services.Contracts
{
    [ServiceContract]
    public interface IOrderService
    {
        [OperationContract]
        void AddOrUpdateCart(string customerID, Order order);

        [OperationContract]
        Order GetCart(string customerID);

        [OperationContract]
        Order GetOrder(int id);

        [OperationContract]
        IEnumerable<Order> GetOrderHistoryByCustomer(string customerID, DateTime startDate, DateTime endDate);

        [OperationContract]
        bool PlaceOrder(Order order, ShippingInfo shipping, PaymentInfo payment);
    }
}