using System;
using BloomSales.Web.Store.Models;

namespace BloomSales.Web.Store.Controllers.Business
{
    internal interface IOrdersRetriever
    {
        OrderDetailsViewModel GetOrderDetails(int orderID);

        OrdersHistoryViewModel GetOrdersHistory(string customerID, DateTime startDate, DateTime endDate);
    }
}