using BloomSales.Data.Entities;
using BloomSales.Services.Proxies;
using BloomSales.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.ServiceModel;

namespace BloomSales.Services.IntegrationTests
{
    [TestClass]
    public class InventoryServiceTests
    {
        private static ServiceHost inventorySvcHost;
        private static ServiceHost locationSvcHost;
        private InventoryClient inventoryClient;

        [ClassCleanup]
        public static void CleanupServices()

        {
            inventorySvcHost.Close();
            locationSvcHost.Close();

            // drop the used databases
            Database.Delete("InventoryDatabase");
            Database.Delete("LocationDatabase");
        }

        [ClassInitialize]
        public static void InitializeServices(TestContext context)
        {
            // initialize databases
            if (Database.Exists("LocationDatabase"))
                Database.Delete("LocationDatabase");

            if (Database.Exists("InventoryDatabase"))
                Database.Delete("InventoryDatabase");

            // spin up the services
            locationSvcHost = new ServiceHost(typeof(LocationService));
            locationSvcHost.Open();
            inventorySvcHost = new ServiceHost(typeof(InventoryService));
            inventorySvcHost.Open();
        }

        [TestMethod]
        [TestCategory(TestType.IntegrationTest)]
        [TestCategory("BloomSales.Services.IntegrationTests.InventoryServiceTests")]
        public void AddCategory_GivenANewCategory_AddsToTheDatabase()
        {
            // arrange
            var category = new ProductCategory()
            {
                Name = "ZZZ"
            };

            // act
            inventoryClient.AddCategory(category);

            // assert
            var temp = inventoryClient.GetCategories();
            var categories = new List<ProductCategory>(temp.OrderBy(p => p.ID));
            Assert.AreEqual("ZZZ", categories[categories.Count - 1].Name);
        }

        [TestMethod]
        [TestCategory(TestType.IntegrationTest)]
        [TestCategory("BloomSales.Services.IntegrationTests.InventoryServiceTests")]
        public void AddProduct_GivenANewProduct_AddsToTheDatabase()
        {
            // arrange
            Product product = new Product()
            {
                Name = "Integration Test New Product",
                CategoryID = 1
            };

            // act
            inventoryClient.AddProduct(product);

            // assert
            var products = inventoryClient.GetAllProducts();
            var testProduct = (from p in products
                               where p.Name == "Integration Test New Product"
                               select p).SingleOrDefault();

            Assert.IsTrue(testProduct != null);
        }

        [TestMethod]
        [TestCategory(TestType.IntegrationTest)]
        [TestCategory("BloomSales.Services.IntegrationTests.InventoryServiceTests")]
        public void AddToInventory_GivenANewInventoryItem_AddsToDatabase()
        {
            // arrange
            var newProduct = new Product() { Name = "TestProduct", CategoryID = 2 };
            inventoryClient.AddProduct(newProduct);
            var products = inventoryClient.GetAllProducts();
            var productList = new List<Product>(products.OrderBy(p => p.ID));
            var productID = productList[productList.Count - 1].ID;

            var item = new InventoryItem()
            {
                ProductID = productID,
                UnitsInStock = 2121,
                WarehouseID = 5
            };

            // act
            inventoryClient.AddToInventory(item);

            // assert
            var actual = inventoryClient.GetStockByWarehouse("BloomSales W#5", productID);
            Assert.AreEqual(2121, actual.UnitsInStock);
        }

        [TestCleanup]
        public void CleanupClient()
        {
            this.inventoryClient.Close();
        }

        [TestMethod]
        [TestCategory(TestType.IntegrationTest)]
        [TestCategory("BloomSales.Services.IntegrationTests.InventoryServiceTests")]
        public void GetAllProcucts_AtAllTimes_ReturnsAListOfAllProducts()
        {
            //arrange

            // act
            var actual = inventoryClient.GetAllProducts();

            // assert
            List<Product> products = new List<Product>(actual);
            Assert.AreEqual(41, products.Count);
        }

        [TestMethod]
        [TestCategory(TestType.IntegrationTest)]
        [TestCategory("BloomSales.Services.IntegrationTests.InventoryServiceTests")]
        public void GetCategories_AtAllTimes_ReturnsListOfProductCategories()
        {
            // arrange

            // act
            var actual = this.inventoryClient.GetCategories();

            // assert
            var categories = new List<ProductCategory>(actual);
            Assert.AreEqual(17, categories.Count);
        }

        [TestMethod]
        [TestCategory(TestType.IntegrationTest)]
        [TestCategory("BloomSales.Services.IntegrationTests.InventoryServiceTests")]
        public void GetInventoryByCity_GivenACity_ReturnsInventoryForThatCity()
        {
            // arrange

            // act
            var actual = this.inventoryClient.GetInventoryByCity("Montreal");

            // assert
            var inventory = new List<InventoryItem>(actual);
            Assert.AreEqual(41, inventory.Count);
        }

        [TestMethod]
        [TestCategory(TestType.IntegrationTest)]
        [TestCategory("BloomSales.Services.IntegrationTests.InventoryServiceTests")]
        public void GetInventoryByRegion_GivenARegionName_ReturnsInventoryForThatRegion()
        {
            // arrange

            // act
            var actual = inventoryClient.GetInventoryByRegion("Western Canada");

            // assert
            var inventory = new List<InventoryItem>(actual);
            Assert.AreEqual(82, inventory.Count);
        }

        [TestMethod]
        [TestCategory(TestType.IntegrationTest)]
        [TestCategory("BloomSales.Services.IntegrationTests.InventoryServiceTests")]
        public void GetInventoryByWarehouse_GiveAWarehouseName_ReturnsInventoryForThatWarehouse()
        {
            // arrange

            // act
            var actual = inventoryClient.GetInventoryByWarehouse("BloomSales W#2");

            // assert
            var inventory = new List<InventoryItem>(actual);
            Assert.AreEqual(41, inventory.Count);
        }

        [TestMethod]
        [TestCategory(TestType.IntegrationTest)]
        [TestCategory("BloomSales.Services.IntegrationTests.InventoryServiceTests")]
        public void GetProductsByCategory_GivenACategoryName_RetunsListOfRelatedProcuts()
        {
            // arrange

            // act
            var actual = inventoryClient.GetProductsByCategory("Fridges");

            // assert
            List<Product> fridges = new List<Product>(actual);
            Assert.AreEqual(1, fridges.Count);
            Assert.AreEqual("Kenmore®/MD 18.2 Cu.Ft Top Mount Refrigerator - White", fridges[0].Name);
        }

        [TestMethod]
        [TestCategory(TestType.IntegrationTest)]
        [TestCategory("BloomSales.Services.IntegrationTests.InventoryServiceTests")]
        public void GetStockByWarehouse_GivenAWarehouseNameAndProductID_ReturnsStockOfTheProductInTheWarehouse()
        {
            // arrange

            // act
            var actual = inventoryClient.GetStockByWarehouse("BloomSales W#5", 34);

            // assert
            Assert.IsTrue(actual.UnitsInStock != 0);
        }

        [TestMethod]
        [TestCategory(TestType.IntegrationTest)]
        [TestCategory("BloomSales.Services.IntegrationTests.InventoryServiceTests")]
        public void GetStocksByCity_GivenACityNameAndProductID_ReturnsStocksOfTheProductInTheCity()
        {
            // arrange

            // act
            var actual = inventoryClient.GetStocksByCity("Halifax", 28);

            // assert
            var stocks = new List<InventoryItem>(actual);
            Assert.AreEqual(1, stocks.Count);
            Assert.IsTrue(stocks[0].UnitsInStock != 0);
        }

        [TestMethod]
        [TestCategory(TestType.IntegrationTest)]
        [TestCategory("BloomSales.Services.IntegrationTests.InventoryServiceTests")]
        public void GetStocksByRegion_GivenARegionAndProductID_ReturnsStocksOfTheProductInTheRegion()
        {
            // arrange

            // act
            var actual = inventoryClient.GetStocksByRegion("Eastern Canada", 20);

            // assert
            var stocks = new List<InventoryItem>(actual.OrderBy(s => s.ID));
            Assert.AreEqual(5, stocks.Count);
            Assert.IsTrue(stocks[0].UnitsInStock != 0);
            Assert.IsTrue(stocks[1].UnitsInStock != 0);
            Assert.IsTrue(stocks[2].UnitsInStock != 0);
            Assert.IsTrue(stocks[3].UnitsInStock != 0);
            Assert.IsTrue(stocks[4].UnitsInStock != 0);
        }

        [TestInitialize]
        public void InitializeClient()

        {
            this.inventoryClient = new InventoryClient();
        }

        [TestMethod]
        [TestCategory(TestType.IntegrationTest)]
        [TestCategory("BloomSales.Services.IntegrationTests.InventoryServiceTests")]
        public void UpdateStock_GivenAnUpdatedItem_UpdateTheDatabaseRecord()
        {
            // arrange
            /*
             * The record being manipulated is:
             *  PRODUCT ID: 12
             *  WAREHOUSE ID: 4
             *  INVENTORY ITEM RECORD ID: 81
            */

            // act
            inventoryClient.UpdateStock(81, 55);

            // assert
            var actual = inventoryClient.GetStockByWarehouse("BloomSales W#4", 12);
            Assert.AreEqual(55, actual.UnitsInStock);
        }
    }
}