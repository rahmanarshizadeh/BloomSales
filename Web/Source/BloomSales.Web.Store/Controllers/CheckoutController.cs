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
            ViewData["provinces"] = locationService.GetAllProvinces("Canada");

            return PartialView();
        }

        [HttpPost]
        public ActionResult Address(ShippingInfo shipping)
        {
            SetShippingInSession(shipping);

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
            var shipping = GetShippingFromSession();
            shipping.ServiceID = shippingServiceID;
            SetShippingInSession(shipping);
            SetShippingCostInSession(serviceCost);

            return RedirectToAction("Payment");
        }

        public ActionResult DeliveryServices(string shipperName)
        {
            var services = shippingService.GetServicesByShipper(shipperName);

            return PartialView(services);
        }

        public ActionResult Payment()
        {
            var userID = User.Identity.GetUserId();
            BillViewModel bill = new BillViewModel();
            bill.Shipping = GetShippingFromSession();
            SetShippingInSession(null);
            bill.Tax = accountingService.GetTaxInfo(bill.Shipping.Country,
                                                    bill.Shipping.Province);
            bill.Order = orderService.GetCart(userID);
            bill.Payment = new PaymentInfo() { Currency = "CAD", Type = PaymentType.CreditCard };
            bill.Payment.Amount = CalculateTotal(bill.OrderSubtotal, bill.Tax);
            bill.ShippingCost = (decimal)GetShippingCostFromSession();
            SetShippingCostInSession(null);

            SetBillInSession(bill);

            return PartialView(bill);
        }

        [HttpPost]
        public ActionResult PaymentMethod(int method)
        {
            var bill = GetBillFromSession();
            bill.Payment.Type = (PaymentType)method;
            SetBillInSession(bill);

            return PartialView();
        }

        [HttpPost]
        public ActionResult Submit()
        {
            var bill = GetBillFromSession();

            var result = orderService.PlaceOrder(bill.Order, bill.Shipping, bill.Payment);

            if (result)
            {
                SetBillInSession(null);
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

        private decimal CalculateTotal(decimal subtotal, SalesTaxInfo tax)
        {
            var totalTax = tax.Federal + tax.Provincial;
            var shipping = (decimal)GetShippingCostFromSession();

            var result = subtotal + shipping;
            result += (result * (decimal)totalTax);

            return result;
        }

        private decimal CalculateSubtotal(IEnumerable<OrderItem> orderItems)
        {
            decimal result = 0;

            foreach (var item in orderItems)
                result += item.Quantity * item.UnitPrice;

            return result;
        }

        private ShippingInfo GetShippingFromSession()
        {
            return Session["shipping"] as ShippingInfo;
        }

        private void SetShippingInSession(ShippingInfo shipping)
        {
            Session["shipping"] = shipping;
        }

        private decimal? GetShippingCostFromSession()
        {
            return Session["shippingCost"] as decimal?;
        }

        private void SetShippingCostInSession(decimal? cost)
        {
            Session["shippingCost"] = cost;
        }

        private BillViewModel GetBillFromSession()
        {
            return Session["bill"] as BillViewModel;
        }

        private void SetBillInSession(BillViewModel bill)
        {
            Session["bill"] = bill;
        }
    }
}