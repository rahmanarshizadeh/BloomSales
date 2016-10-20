using BloomSales.Data.Entities;
using BloomSales.Services.Contracts;
using BloomSales.Services.Proxies;
using BloomSales.Web.Store.Controllers.Business;
using BloomSales.Web.Store.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
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
            var cart = GetShoppingCart();
            var productIDs = cart.ItemsProductIDs;

            var products = inventoryService.GetProductsByIDs(productIDs);
            var items = new List<OrderItem>(cart.Items);

            Tuple<List<OrderItem>, IEnumerable<Product>> cartItems =
                new Tuple<List<OrderItem>, IEnumerable<Product>>(items, products);

            return View(cartItems);
        }

        public ActionResult Count()
        {
            var cart = GetShoppingCart();

            return PartialView(cart.NumberOfItems);
        }

        [HttpPost]
        public ActionResult Add(ProductDetailsViewModel productDetails)
        {
            OrderItem item = new OrderItem();
            item.ProductID = productDetails.ID;
            item.Quantity = 1;
            item.UnitPrice = productDetails.UnitPrice;

            var cart = GetShoppingCart();
            cart.AddItem(item);

            SetShoppingCart(cart);

            return RedirectToAction("Count");
        }

        [HttpPost]
        public ActionResult Update(List<OrderItem> items)
        {
            var cart = GetShoppingCart();
            cart.Update(items);

            SetShoppingCart(cart);

            return RedirectToAction("Calculate");
        }

        public ActionResult Calculate()
        {
            var cart = GetShoppingCart();
            var subtotal = cart.Subtotal;

            return PartialView(subtotal);
        }

        public ActionResult CheckoutTransition()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Checkout");
            else
                return RedirectToAction("SaveCart");
        }

        [Authorize]
        public ActionResult SaveCart()
        {
            var sessionHandler = new SessionHandler(Session);
            Order order = sessionHandler.Cart;

            SetShoppingCart(new Cart(order));

            sessionHandler.DeleteCart();

            return RedirectToAction("Index", "Checkout");
        }

        private void SetShoppingCart(Cart cart)
        {
            if (User.Identity.IsAuthenticated)
                orderService.AddOrUpdateCart(User.Identity.GetUserId(), cart.Order);
            else
            {
                var sessionHandler = new SessionHandler(Session);
                sessionHandler.Cart = cart.Order;
            }
        }

        private Cart GetShoppingCart()
        {
            Order order;
            if (User.Identity.IsAuthenticated)
            {
                order = orderService.GetCart(User.Identity.GetUserId());
            }
            else
            {
                var sessionHandler = new SessionHandler(Session);
                order = sessionHandler.Cart;
            }
            return new Cart(order);
        }
    }
}