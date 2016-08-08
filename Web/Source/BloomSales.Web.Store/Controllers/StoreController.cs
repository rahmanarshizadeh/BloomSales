using BloomSales.Data.Entities;
using BloomSales.Services.Contracts;
using BloomSales.Services.Proxies;
using BloomSales.Web.Store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BloomSales.Web.Store.Controllers
{
    public class StoreController : Controller
    {
        private IInventoryService inventoryService;
        private ILocationService locationService;

        public StoreController()
        {
            //this.inventoryService = inventoryServiceProxy;
            this.inventoryService = new InventoryClient();
            this.locationService = new LocationClient();
        }

        public ActionResult Index(string categoryName = "")
        {
            return View((object)categoryName);
        }

        public ActionResult CategoriesList()
        {
            var allCategories = inventoryService.GetCategories();
            var categoriesList = new Dictionary<string, List<string>>();

            // put the children under each parent and create the full object tree
            foreach (var cat in allCategories)
            {
                if (cat.Parent != null)
                {
                    if (!categoriesList.ContainsKey(cat.Parent.Name))
                        categoriesList.Add(cat.Parent.Name, new List<string>());

                    categoriesList[cat.Parent.Name].Add(cat.Name);
                }
            }

            // sort the children
            foreach (var key in categoriesList.Keys)
            {
                var children = categoriesList[key];
                children.Sort();
            }

            return PartialView(categoriesList);
        }

        public ActionResult Products(string categoryName)
        {
            var products = inventoryService.GetProductsByCategory(categoryName);

            return PartialView(products);
        }

        public ActionResult ProductDetails(int id, decimal unitPrice)
        {
            Tuple<int, decimal> model = new Tuple<int, decimal>(id, unitPrice);

            return PartialView(model);
        }
    }
}