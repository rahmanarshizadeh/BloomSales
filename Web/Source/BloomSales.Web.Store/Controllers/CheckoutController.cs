using BloomSales.Data.Entities;
using BloomSales.Services.Contracts;
using BloomSales.Services.Proxies;
using BloomSales.Web.Store.Controllers.Business;
using BloomSales.Web.Store.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Web.Mvc;

namespace BloomSales.Web.Store.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private IOrderService orderService;
        private IAccountingService accountingService;
        private ILocationService locationService;
        private IShippingService shippingService;

        public CheckoutController()
        {
            orderService = new OrderClient();
            accountingService = new AccountingClient();
            locationService = new LocationClient();
            shippingService = new ShippingClient();
        }

        public ActionResult Index()
        {
            var userID = User.Identity.GetUserId();
            var order = orderService.GetCart(userID);
            order.CustomerID = userID;
            orderService.AddOrUpdateCart(userID, order);

            return View();
        }

        public ActionResult Address()
        {
            var provinces = locationService.GetAllProvinces("Canada");

            return PartialView(provinces);
        }

        [HttpPost]
        public ActionResult Address(ShippingInfo shipping)
        {
            var sessionHandler = new SessionHandler(Session);
            sessionHandler.Shipping = shipping;

            return RedirectToAction("Shipping");
        }

        public ActionResult Shipping()
        {
            var shippers = shippingService.GetAllShippers();
            var shippersList = new List<Shipper>(shippers);
            shippersList.RemoveAt(0);

            return PartialView(shippersList);
        }

        [HttpPost]
        public ActionResult Shipping(int shippingServiceID, decimal serviceCost)
        {
            var sessionHandler = new SessionHandler(Session);
            var shipping = sessionHandler.Shipping;
            shipping.ServiceID = shippingServiceID;
            sessionHandler.Shipping = shipping;
            sessionHandler.ShippingCost = serviceCost;

            return RedirectToAction("Payment");
        }

        public ActionResult DeliveryServices(string shipperName)
        {
            var services = shippingService.GetServicesByShipper(shipperName);

            return PartialView(services);
        }

        public ActionResult Payment()
        {
            var sessionHandler = new SessionHandler(Session);
            var userID = User.Identity.GetUserId();
            BillViewModel bill = new BillViewModel();
            bill.Shipping = sessionHandler.Shipping;
            sessionHandler.Shipping = null;
            bill.Tax = accountingService.GetTaxInfo(bill.Shipping.Country,
                                                    bill.Shipping.Province);
            bill.Order = orderService.GetCart(userID);
            bill.Payment = new PaymentInfo() { Currency = "CAD", Type = PaymentType.CreditCard };
            bill.ShippingCost = (decimal)sessionHandler.ShippingCost;
            bill.Payment.Amount = bill.OrderTotal;
            bill.ShippingCost = (decimal)sessionHandler.ShippingCost;
            sessionHandler.ShippingCost = null;

            sessionHandler.Bill = bill;

            return PartialView(bill);
        }

        [HttpPost]
        public ActionResult PaymentMethod(int method)
        {
            var sessionHandler = new SessionHandler(Session);
            var bill = sessionHandler.Bill;
            bill.Payment.Type = (PaymentType)method;
            sessionHandler.Bill = bill;

            return PartialView();
        }

        [HttpPost]
        public ActionResult Submit()
        {
            var sessionHandler = new SessionHandler(Session);
            var bill = sessionHandler.Bill;

            var result = orderService.PlaceOrder(bill.Order, bill.Shipping, bill.Payment);

            if (result)
            {
                sessionHandler.Bill = null;
                return RedirectToAction("Success");
            }
            else
            {
                return RedirectToAction("Failure");
            }
        }

        public ActionResult Success()
        {
            return View("Success");
        }

        public ActionResult Failure()
        {
            return View("Failure");
        }
    }
}