using BloomSales.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloomSales.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private OrderDb db;

        public OrderRepository()
        {
            this.db = new OrderDb();
        }

        internal OrderRepository(OrderDb context)
        {
            this.db = context;
        }

        public int AddOrder(Order order)
        {
            this.db.Orders.Add(order);
            this.db.SaveChanges();

            return order.ID;
        }

        public void UpdateOrder(Order order)
        {
            var record = (from o in db.Orders
                          where order.ID == o.ID
                          select o).Single();

            record.IsInternalOrder = order.IsInternalOrder;
            record.Items = order.Items;
            record.ParentOrderID = order.ParentOrderID;
            record.HasProcessed = order.HasProcessed;

            db.SaveChanges();
        }

        public Order GetOrder(int id)
        {
            var order = this.db.Orders.Find(id);

            order.Items = GetOrderItems(id);

            order.SubOrders = (from o in db.Orders
                               where o.ParentOrderID == id
                               select o).ToArray();

            foreach (Order o in order.SubOrders)
                o.Items = GetOrderItems(o.ID);

            return order;
        }

        public IEnumerable<Order> GetOrdersByCustomer(int cusotmerID, DateTime startDate, DateTime endDate)
        {
            var result = (from o in db.Orders
                          where o.CustomerID == cusotmerID &&
                                o.OrderDate > startDate &&
                                o.OrderDate < endDate
                          select o).ToArray();

            return result;
        }

        public void Dispose()
        {
            if (this.db != null)
                db.Dispose();
        }

        private OrderItem[] GetOrderItems(int orderID)
        {
            return (from item in db.OrderItems
                    where item.OrderID == orderID
                    select item).ToArray();
        }
    }
}