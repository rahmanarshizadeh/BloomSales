using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BloomSales.Data.Entities;

namespace BloomSales.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private OrdersDb db;

        public OrderRepository()
        {
            this.db = new OrdersDb();
        }

        internal OrderRepository(OrdersDb context)
        {
            this.db = context;
        }

        public int AddOrder(Order order)
        {
            this.db.Orders.Add(order);
            this.db.SaveChanges();

            return order.ID;
        }

        public Order GetOrder(int id)
        {
            return this.db.Orders.Find(id);
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
    }
}
