using BloomSales.Data.Entities;
using System;
using System.Collections.Generic;

namespace BloomSales.Data.Repositories
{
    public interface IOrderRepository : IRepository
    {
        int AddOrder(Order order);

        Order GetOrder(int id);

        IEnumerable<Order> GetOrdersByCustomer(string cusotmerID, DateTime startDate, DateTime endDate);

        void UpdateOrder(Order order);
    }
}