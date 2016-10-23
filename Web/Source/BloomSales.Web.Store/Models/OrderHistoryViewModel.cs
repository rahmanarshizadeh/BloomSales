using BloomSales.Data.Entities;
using System.Collections.Generic;
using System;
using System.Linq;

namespace BloomSales.Web.Store.Models
{
    public class OrderHistoryViewModel : OrderViewModelBase
    {
        public class OrderHistoryItem
        {
            public OrderHistoryItem(int orderID, DateTime orderDate,
                                    decimal total, string shippingStatus)
            {
                OrderID = orderID;
                OrderDate = orderDate;
                TotalPayment = total;
                ShippingStatus = shippingStatus;
            }

            public int OrderID { get; private set; }
            public DateTime OrderDate { get; private set; }
            public decimal TotalPayment { get; private set; }
            public string ShippingStatus { get; private set; }
        }

        private IList<OrderHistoryItem> history;

        public OrderHistoryViewModel(IEnumerable<Order> orders,
                                     IEnumerable<ShippingStatus> statuses,
                                     IEnumerable<PaymentInfo> payments)
        {
            history = new List<OrderHistoryItem>();
            OrderHistoryItem item;
            var statusIt = statuses.GetEnumerator();
            var paymentIt = payments.GetEnumerator();

            foreach (var order in orders)
            {
                statusIt.MoveNext();
                paymentIt.MoveNext();
                string status = GetStatusTitle(statusIt.Current);
                item = new OrderHistoryItem(order.ID, order.OrderDate,
                                            paymentIt.Current.Amount, status);
                history.Add(item);
            }
        }

        public IEnumerable<OrderHistoryItem> OrderHistory
        {
            get
            {
                // return the history in descending order
                return history.OrderByDescending(o => o.OrderDate).AsEnumerable();
            }
        }
    }
}