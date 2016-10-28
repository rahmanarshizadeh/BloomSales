using BloomSales.Services.Proxies;
using BloomSales.Web.Store.Controllers.Business;
using Microsoft.AspNet.Identity;
using System;
using System.Web.Mvc;

namespace BloomSales.Web.Store.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private IOrdersRetriever orderRetriever;

        public OrderController()
        {
            orderRetriever = new OrdersRetriever(new OrderClient(), new AccountingClient(),
                                                 new ShippingClient(), new InventoryClient());
        }

        public ActionResult History()
        {
            var history = orderRetriever.GetOrdersHistory(User.Identity.GetUserId(),
                                                          DateTime.MinValue, DateTime.MaxValue);

            if (history == null)
                return View("EmptyHistory");

            return View(history);
        }

        public ActionResult Details(int orderID)
        {
            var orderDetials = orderRetriever.GetOrderDetails(orderID);

            return PartialView(orderDetials);
        }
    }
}