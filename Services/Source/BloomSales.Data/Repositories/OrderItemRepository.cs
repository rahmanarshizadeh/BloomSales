using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BloomSales.Data.Entities;

namespace BloomSales.Data.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private OrdersDb db;

        public OrderItemRepository()
        {
            this.db = new OrdersDb();
        }

        internal OrderItemRepository(OrdersDb context)
        {
            this.db = context;
        }

        public void AddItem(OrderItem item)
        {
            this.db.OrderItems.Add(item);
            this.db.SaveChanges();
        }

        public void AddItems(IEnumerable<OrderItem> items)
        {
            this.db.OrderItems.AddRange(items);
            this.db.SaveChanges();
        }

        public void RemoveItem(int id)
        {
            OrderItem item = this.db.OrderItems.Find(id);
            this.db.OrderItems.Remove(item);
            this.db.SaveChanges();
        }

        public IEnumerable<OrderItem> GetItemsByOrder(int orderID)
        {
            var result = (from i in db.OrderItems
                          where i.OrderID == orderID
                          select i).ToArray();

            return result;
        }

        public void Dispose()
        {
            if (this.db != null)
                db.Dispose();
        }
    }
}
