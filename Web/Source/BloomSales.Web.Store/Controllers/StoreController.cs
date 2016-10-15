using BloomSales.Services.Contracts;
using BloomSales.Services.Proxies;
using BloomSales.Web.Store.Controllers.Business;
using System;
using System.Web.Mvc;

namespace BloomSales.Web.Store.Controllers
{
    public class StoreController : Controller
    {
        private IInventoryService inventoryService;
        private ILocationService locationService;

        public StoreController()
        {
            inventoryService = new InventoryClient();
            locationService = new LocationClient();
        }

        public ActionResult Index(string categoryName = "")
        {
            return View((object)categoryName);
        }

        public ActionResult Catalog(string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName))
                return PartialView("CatalogFrontPage");
            else
                return Products(categoryName);
        }

        public ActionResult CategoriesList()
        {
            var retriever = new CategoriesRetriever(inventoryService);
            var categoriesList = retriever.GetList();
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