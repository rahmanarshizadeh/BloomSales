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
    public class OrderClient : ClientBase<IOrderService>, IOrderService
    {
        public Order GetOrder(int id)
        {
            return Channel.GetOrder(id);
        }

        public IEnumerable<Order> GetOrderHistoryByCustomer(string customerID, DateTime startDate, DateTime endDate)
        {
            return Channel.GetOrderHistoryByCustomer(customerID, startDate, endDate);
        }

        public bool PlaceOrder(Order order, ShippingInfo shipping, PaymentInfo payment)
        {
            return Channel.PlaceOrder(order, shipping, payment);
        }

        public void AddOrUpdateCart(string customerID, Order order)
        {
            Channel.AddOrUpdateCart(customerID, order);
        }

        public Order GetCart(string customerID)
        {
            return Channel.GetCart(customerID);
        }
    }
}