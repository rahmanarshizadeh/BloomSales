using BloomSales.Data.Entities;
using System.Collections.Generic;

namespace BloomSales.Data.Repositories
{
    public interface IOrderItemRepository : IRepository
    {
        void AddItem(OrderItem item);

        void AddItems(IEnumerable<OrderItem> items);

        IEnumerable<OrderItem> GetItemsByOrder(int orderID);

        void RemoveItem(int id);
    }
}