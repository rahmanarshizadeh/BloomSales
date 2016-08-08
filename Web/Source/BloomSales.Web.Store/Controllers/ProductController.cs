using BloomSales.Services.Contracts;
using BloomSales.Services.Proxies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BloomSales.Web.Store.Controllers
{
    public class ProductController : Controller
    {
        private IInventoryService inventoryService;

        public ProductController()
        {
            inventoryService = new InventoryClient();
        }

        public ActionResult Details(int id)
        {
            var product = inventoryService.GetProductByID(id);

            return PartialView(product);
        }
    }
}