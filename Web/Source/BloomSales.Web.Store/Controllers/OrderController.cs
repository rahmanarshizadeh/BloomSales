using BloomSales.Data.Entities;
using BloomSales.Services.Contracts;
using BloomSales.Services.Proxies;
using BloomSales.Web.Store.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BloomSales.Web.Store.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private IOrderService orderService;
        private IAccountingService accountingService;
        private IShippingService shippingService;
        private IInventoryService inventoryService;

        public OrderController()
        {
            orderService = new OrderClient();
            accountingService = new AccountingClient();
            shippingService = new ShippingClient();
            inventoryService = new InventoryClient();
        }

        public ActionResult History()
        {
            var orders = orderService.GetOrderHistoryByCustomer(User.Identity.GetUserId(),
                                                                DateTime.MinValue,
                                                                DateTime.Today);
            var payments = new List<PaymentInfo>();
            var shippingStatuses = new List<string>();
            PaymentInfo payment;
            ShippingStatus status;
            string statusTitle;

            if (orders.Count() == 0)
                return View("EmptyHistory");

            foreach (var order in orders)
            {
                payment = accountingService.GetPaymentFor(order.ID);
                payments.Add(payment);
                status = shippingService.GetShippingStatus(order.ID);
                statusTitle = GetStatusTitle(status);
                shippingStatuses.Add(statusTitle);
            }

            Tuple<IEnumerable<Order>, List<PaymentInfo>, List<string>> model =
                new Tuple<IEnumerable<Order>, List<PaymentInfo>, List<string>>(orders,
                                                                               payments,
                                                                               shippingStatuses);

            return View(model);
        }

        public ActionResult Details(int orderID)
        {
            Order order = orderService.GetOrder(orderID);
            var shippings = new List<ShippingInfo>();
            GetShippings(order, shippings);
            var products = GetProducts(order);

            if (order.Items == null)
                AggregateItems(order);

            var statuses = GetStatusTitles(shippings);

            BillViewModel bill = new BillViewModel()
            {
                Order = order,
                OrderSubtotal = CalculateSubtotal(order.Items),
                Payment = accountingService.GetPaymentFor(orderID),
                Shipping = shippings[0],
                ShippingCost = shippings[0].Service.Cost,
                Tax = accountingService.GetTaxInfo(shippings[0].Country, shippings[0].Province)
            };

            ViewData["products"] = products;
            var model =
                new Tuple<BillViewModel, IEnumerable<ShippingInfo>, string, IEnumerable<string>>(
                                                        bill, shippings,
                                                        GetPaymentMethodTitle(bill.Payment.Type),
                                                        statuses);

            return PartialView(model);
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

        private IEnumerable<string> GetStatusTitles(List<ShippingInfo> shippings)
        {
            var list = new List<string>();

            foreach (var shipping in shippings)
                list.Add(GetStatusTitle(shipping.Status));

            return list;
        }

        private decimal CalculateSubtotal(IEnumerable<OrderItem> orderItems)
        {
            decimal result = 0;

            foreach (var item in orderItems)
                result += item.Quantity * item.UnitPrice;

            return result;
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

        private string GetPaymentMethodTitle(PaymentType type)
        {
            switch (type)
            {
                case PaymentType.CreditCard:
                    return "Credit Card";

                case PaymentType.OnlineBanking:
                    return "Online Banking";

                default:
                    return type.ToString();
            }
        }

        private void AggregateItems(Order order)
        {
            List<OrderItem> items = new List<OrderItem>();

            foreach (var subOrder in order.SubOrders)
                foreach (var item in subOrder.Items)
                    items.Add(item);

            order.Items = items;
        }

        private string GetStatusTitle(ShippingStatus status)
        {
            switch (status)
            {
                case ShippingStatus.OutForDelivery:
                    return "Out for Delivery";

                case ShippingStatus.PickedUp:
                    return "Picked up";

                case ShippingStatus.ReceivedOrder:
                    return "Received order";

                default:
                    return status.ToString();
            }
        }
    }
}