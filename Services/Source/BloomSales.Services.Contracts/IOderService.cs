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
    public interface IOderService
    {
        bool PlaceOrder(Order order, ShippingInfo shipping, PaymentInfo payment);

        Order GetOrder(int id);

        IEnumerable<Order> GetOrderHistoryByCustomer(int customerID, DateTime startDate, DateTime endDate);
    }
}
