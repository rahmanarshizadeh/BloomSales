using BloomSales.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloomSales.Data.Repositories
{
    public interface IOrderItemRepository : IRepository
    {
        void AddItem(OrderItem item);

        void AddItems(IEnumerable<OrderItem> items);

        void RemoveItem(int id);

        IEnumerable<OrderItem> GetItemsByOrder(int orderID);
    }
}
