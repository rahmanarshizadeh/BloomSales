using BloomSales.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloomSales.Data.Repositories
{
    public interface IOrderRepository : IRepository
    {
        int AddOrder(Order order);

        void UpdateOrder(Order order);

        Order GetOrder(int id);

        IEnumerable<Order> GetOrdersByCustomer(int cusotmerID, DateTime startDate, DateTime endDate);
    }
}