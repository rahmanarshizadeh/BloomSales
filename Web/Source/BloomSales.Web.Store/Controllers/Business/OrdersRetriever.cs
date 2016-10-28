using BloomSales.Data.Entities;
using BloomSales.Services.Contracts;
using BloomSales.Web.Store.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BloomSales.Web.Store.Controllers.Business
{
    public class OrdersRetriever : IOrdersRetriever
    {
        private IOrderService orderService;
        private IAccountingService accountingService;
        private IShippingService shippingService;
        private IInventoryService inventoryService;

        public OrdersRetriever(IOrderService orderService,
                               IAccountingService accountingService,
                               IShippingService shippingService,
                               IInventoryService inventoryService)
        {
            this.orderService = orderService;
            this.accountingService = accountingService;
            this.shippingService = shippingService;
            this.inventoryService = inventoryService;
        }

        public OrderDetailsViewModel GetOrderDetails(int orderID)
        {
            Order order = orderService.GetOrder(orderID);
            var shippings = new List<ShippingInfo>();
            GetShippings(order, shippings);
            var products = GetProducts(order);

            if (order.Items == null)
                AggregateItems(order);

            BillViewModel bill = new BillViewModel()
            {
                Order = order,
                Payment = accountingService.GetPaymentFor(orderID),
                Shipping = shippings[0],
                ShippingCost = shippings[0].Service.Cost,
                Tax = accountingService.GetTaxInfo(shippings[0].Country, shippings[0].Province)
            };

            return new OrderDetailsViewModel(bill, shippings, products); ;
        }

        public OrdersHistoryViewModel GetOrdersHistory(string customerID, DateTime startDate,
                                                      DateTime endDate)
        {
            var orders = orderService.GetOrderHistoryByCustomer(customerID, startDate, endDate);
            var payments = new List<PaymentInfo>();
            var shippingStatuses = new List<ShippingStatus>();
            PaymentInfo payment;
            ShippingStatus status;

            if (orders.Count() == 0)
                return null;

            foreach (var order in orders)
            {
                payment = accountingService.GetPaymentFor(order.ID);
                payments.Add(payment);
                status = shippingService.GetShippingStatus(order.ID);
                shippingStatuses.Add(status);
            }

            OrdersHistoryViewModel history =
                new OrdersHistoryViewModel(orders, shippingStatuses, payments);

            return history;
        }

        private void GetShippings(Order order, List<ShippingInfo> shippings)
        {
            if (order.SubOrders.Count() == 0)
                order.SubOrders = new Order[1] { order };

            foreach (var o in order.SubOrders)
            {
                var shipping = shippingService.GetShipping(o.ID);
                shippings.Add(shipping);
            }
        }

        private IEnumerable<IEnumerable<Product>> GetProducts(Order order)
        {
            List<IEnumerable<Product>> products = new List<IEnumerable<Product>>();
            List<int> ids = new List<int>();

            foreach (var subOrder in order.SubOrders)
            {
                foreach (var item in subOrder.Items)
                    ids.Add(item.ProductID);

                var pList = inventoryService.GetProductsByIDs(ids);
                products.Add(pList);

                ids.Clear();
            }

            return products;
        }

        private void AggregateItems(Order order)
        {
            List<OrderItem> items = new List<OrderItem>();

            foreach (var subOrder in order.SubOrders)
                foreach (var item in subOrder.Items)
                    items.Add(item);

            order.Items = items;
        }
    }
}