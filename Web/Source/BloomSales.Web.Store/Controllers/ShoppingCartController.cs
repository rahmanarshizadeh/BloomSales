using BloomSales.Data.Entities;
using BloomSales.Services.Contracts;
using BloomSales.Services.Proxies;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BloomSales.Web.Store.Controllers
{
    public class ShoppingCartController : Controller
    {
        private IOrderService orderService;
        private IInventoryService inventoryService;

        public ShoppingCartController()
        {
            orderService = new OrderClient();
            inventoryService = new InventoryClient();
        }

        // GET: ShoppingCart
        public ActionResult Index()
        {
            var order = GetCartFromSession();
            var productIDs = new List<int>();

            foreach (var item in order.Items)
                productIDs.Add(item.ProductID);

            var products = inventoryService.GetProductsByIDs(productIDs);
            var items = order.Items as List<OrderItem>;

            Tuple<List<OrderItem>, IEnumerable<Product>> cartItems =
                new Tuple<List<OrderItem>, IEnumerable<Product>>(items, products);

            return View(cartItems);
        }

        public ActionResult Count(int customerID = 0)
        {
            Order order = null;

            order = GetCartFromSession();

            if (order == null)
                return PartialView(0);

            int count = order.Items.Count();
            return PartialView(count);
        }

        [HttpPost]
        public ActionResult Add(int productItemID, decimal unitPrice)
        {
            OrderItem item = new OrderItem();
            item.ProductID = productItemID;
            item.Quantity = 1;
            item.UnitPrice = unitPrice;

            Order order = GetCartFromSession();
            List<OrderItem> items;

            if (order == null)
            {
                order = new Order();
                items = new List<OrderItem>();
            }
            else
            {
                items = order.Items as List<OrderItem>;
            }

            AddItemToCart(items, item);

            order.Items = items;

            SetCartInSession(order);

            return RedirectToAction("Count");
        }

        [HttpPost]
        public ActionResult Update(List<OrderItem> items)
        {
            List<OrderItem> updatedItemsList = new List<OrderItem>();

            foreach (OrderItem item in items)
                if (item.Quantity > 0)
                    updatedItemsList.Add(item);

            var order = GetCartFromSession();
            order.Items = updatedItemsList;

            return RedirectToAction("Calculate");
        }

        public ActionResult Calculate()
        {
            var order = GetCartFromSession();
            decimal subtotal = 0;

            foreach (var item in order.Items)
                subtotal += item.Quantity * item.UnitPrice;

            return PartialView(subtotal);
        }

        private void AddItemToCart(List<OrderItem> items, OrderItem item)
        {
            var duplicate = items.Find(i => i.ProductID == item.ProductID);

            if (duplicate != null)
                duplicate.Quantity++;
            else
                items.Add(item);
        }

        private Order GetCartFromSession()
        {
            return Session["cart"] as Order;
        }

        private void SetCartInSession(Order order)
        {
            Session["cart"] = order;
        }
    }
}