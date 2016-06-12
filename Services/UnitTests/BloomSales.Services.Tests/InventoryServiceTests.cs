using BloomSales.Data.Entities;
using BloomSales.Data.Repositories;
using BloomSales.Services.Contracts;
using BloomSales.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace BloomSales.Services.Tests
{
    [TestClass]
    public class InventoryServiceTests
    {
        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        [TestCategory("BloomSales.Services.Tests.InventoryServiceTests")]
        public void GetAllProducts_ResultExistsInCache_ReturnsTheResult()
        {
            // arrange
            List<Product> expected = new List<Product>();
            expected.Add(new Product() { ID = 1 });
            expected.Add(new Product() { ID = 2 });
            expected.Add(new Product() { ID = 3 });
            expected.Add(new Product() { ID = 4 });
            expected.Add(new Product() { ID = 5 });
            Mock<ObjectCache> mockCache = new Mock<ObjectCache>();
            mockCache.Setup(c => c["allProducts"]).Returns(expected);
            InventoryService sut = new InventoryService(null, null, null, mockCache.Object, null);

            // act
            var actual = sut.GetAllProducts();

            // assert
            Assert.IsTrue(Equality.AreEqual(expected, actual));
            mockCache.Verify(c => c["allProducts"], Times.Once());
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        [TestCategory("BloomSales.Services.Tests.InventoryServiceTests")]
        public void GetAllProducts_ResultNotExistsInCache_FetchesTheResultFromDatabase()
        {
            // arrange
            List<Product> expected = new List<Product>();
            expected.Add(new Product() { ID = 1 });
            expected.Add(new Product() { ID = 2 });
            expected.Add(new Product() { ID = 3 });
            expected.Add(new Product() { ID = 4 });
            expected.Add(new Product() { ID = 5 });
            Mock<IProductRepository> mockProductRepo = new Mock<IProductRepository>();
            mockProductRepo.Setup(r => r.GetAllProducts()).Returns(expected);
            Mock<ObjectCache> mockCache = new Mock<ObjectCache>();
            InventoryService sut = new InventoryService(null, null, mockProductRepo.Object, mockCache.Object, null);

            // act
            var actual = sut.GetAllProducts();

            // assert
            Assert.IsTrue(Equality.AreEqual(expected, actual));
            mockProductRepo.Verify(r => r.GetAllProducts(), Times.Once());
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        [TestCategory("BloomSales.Services.Tests.InventoryServiceTests")]
        public void GetAllProducts_AfterFetchingResultFromDatabase_CachesTheResult()
        {
            // arrange
            List<Product> expected = new List<Product>();
            expected.Add(new Product() { ID = 1 });
            expected.Add(new Product() { ID = 2 });
            expected.Add(new Product() { ID = 3 });
            expected.Add(new Product() { ID = 4 });
            expected.Add(new Product() { ID = 5 });
            Mock<IProductRepository> mockProductRepo = new Mock<IProductRepository>();
            mockProductRepo.Setup(r => r.GetAllProducts()).Returns(expected);
            Mock<ObjectCache> mockCache = new Mock<ObjectCache>();
            InventoryService sut = new InventoryService(null, null, mockProductRepo.Object, mockCache.Object, null);

            // act
            var actual = sut.GetAllProducts();

            // assert
            mockCache.Verify(
                c => c.Set("allProducts", It.Is<IEnumerable<Product>>(l => l.Count() == 5),
                           It.IsAny<CacheItemPolicy>(), null),
                Times.Once());
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        [TestCategory("BloomSales.Services.Tests.InventoryServiceTests")]
        public void GetProductByCategory_ResultExistsInCache_ReturnsTheResult()
        {
            // arrange
            List<Product> expected = new List<Product>();
            expected.Add(new Product() { ID = 1 });
            expected.Add(new Product() { ID = 2 });
            expected.Add(new Product() { ID = 3 });
            expected.Add(new Product() { ID = 4 });
            Mock<ObjectCache> mockCache = new Mock<ObjectCache>();
            mockCache.Setup(c => c["allProductsInHealthCategory"]).Returns(expected);
            InventoryService sut = new InventoryService(null, null, null, mockCache.Object, null);

            // act
            var actual = sut.GetProductsByCategory("Health");

            // assert
            Assert.IsTrue(Equality.AreEqual(expected, actual));
            mockCache.Verify(c => c["allProductsInHealthCategory"], Times.Once());
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        [TestCategory("BloomSales.Services.Tests.InventoryServiceTests")]
        public void GetProductsByCategory_ResultNotExistsInCache_FetchesTheResultFromDatabase()
        {
            // arrange
            List<Product> expected = new List<Product>();
            expected.Add(new Product() { ID = 1 });
            expected.Add(new Product() { ID = 2 });
            expected.Add(new Product() { ID = 3 });
            ProductCategory category = new ProductCategory() { ID = 6, Name = "Health" };
            Mock<IProductCategoryRepository> mockCategoryRepo = new Mock<IProductCategoryRepository>();
            mockCategoryRepo.Setup(r => r.GetCategory("Health")).Returns(category);
            Mock<IProductRepository> mockProductRepo = new Mock<IProductRepository>();
            mockProductRepo.Setup(r => r.GetProducts(6)).Returns(expected);
            Mock<ObjectCache> mockCache = new Mock<ObjectCache>();
            InventoryService sut = new InventoryService(null, mockCategoryRepo.Object,
                mockProductRepo.Object, mockCache.Object, null);

            // act
            var actual = sut.GetProductsByCategory("Health");

            // assert
            Assert.IsTrue(Equality.AreEqual(expected, actual));
            mockCategoryRepo.Verify(r => r.GetCategory("Health"), Times.Once());
            mockProductRepo.Verify(r => r.GetProducts(6), Times.Once());
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        [TestCategory("BloomSales.Services.Tests.InventoryServiceTests")]
        public void GetProductsByCategory_AfterFetchingResultFromDatabase_CachesTheResult()
        {
            // arrange
            List<Product> expected = new List<Product>();
            expected.Add(new Product() { ID = 1 });
            expected.Add(new Product() { ID = 2 });
            expected.Add(new Product() { ID = 3 });
            ProductCategory category = new ProductCategory() { ID = 6, Name = "Health" };
            Mock<IProductCategoryRepository> mockCategoryRepo = new Mock<IProductCategoryRepository>();
            mockCategoryRepo.Setup(r => r.GetCategory("Health")).Returns(category);
            Mock<IProductRepository> mockProductRepo = new Mock<IProductRepository>();
            mockProductRepo.Setup(r => r.GetProducts(6)).Returns(expected);
            Mock<ObjectCache> mockCache = new Mock<ObjectCache>();
            InventoryService sut = new InventoryService(null, mockCategoryRepo.Object,
                mockProductRepo.Object, mockCache.Object, null);

            // act
            var actual = sut.GetProductsByCategory("Health");

            // assert
            mockCache.Verify(
                c => c.Set(It.Is<string>(s => s.Equals("allProductsInHealthCategory")),
                    It.Is<IEnumerable<Product>>(l => l.Count() == 3),
                    It.IsAny<CacheItemPolicy>(),
                    null),
                Times.Once());
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        [TestCategory("BloomSales.Services.Tests.InventoryServiceTests")]
        public void GetCategories_ResultExistsInCache_ReturnsTheResult()
        {
            // arrange
            List<ProductCategory> expected = new List<ProductCategory>();
            expected.Add(new ProductCategory() { ID = 1 });
            expected.Add(new ProductCategory() { ID = 2 });
            expected.Add(new ProductCategory() { ID = 3 });
            expected.Add(new ProductCategory() { ID = 4 });
            Mock<ObjectCache> mockCache = new Mock<ObjectCache>();
            mockCache.Setup(c => c["allCategories"]).Returns(expected);
            InventoryService sut = new InventoryService(null, null, null, mockCache.Object, null);

            // act
            var actual = sut.GetCategories();

            // assert
            Assert.IsTrue(Equality.AreEqual(expected, actual));
            mockCache.Verify(c => c["allCategories"], Times.Once());
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        [TestCategory("BloomSales.Services.Tests.InventoryServiceTests")]
        public void GetCategories_ResultNotExistsInCache_FetchesTheResultFromDatabase()
        {
            // arrange
            List<ProductCategory> expected = new List<ProductCategory>();
            expected.Add(new ProductCategory() { ID = 1 });
            expected.Add(new ProductCategory() { ID = 2 });
            expected.Add(new ProductCategory() { ID = 3 });
            Mock<IProductCategoryRepository> mockCategoryRepo = new Mock<IProductCategoryRepository>();
            mockCategoryRepo.Setup(r => r.GetAllCategories()).Returns(expected);
            Mock<ObjectCache> mockCache = new Mock<ObjectCache>();
            InventoryService sut = new InventoryService(null, mockCategoryRepo.Object, null, mockCache.Object, null);

            // act
            var actual = sut.GetCategories();

            // assert
            Assert.IsTrue(Equality.AreEqual(expected, actual));
            mockCategoryRepo.Verify(r => r.GetAllCategories(), Times.Once());
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        [TestCategory("BloomSales.Services.Tests.InventoryServiceTests")]
        public void GetCategories_AfterFetchingResultFromDatabase_CachesTheResult()
        {
            // arrange
            List<ProductCategory> expected = new List<ProductCategory>();
            expected.Add(new ProductCategory() { ID = 1 });
            expected.Add(new ProductCategory() { ID = 2 });
            expected.Add(new ProductCategory() { ID = 3 });
            Mock<IProductCategoryRepository> mockCategoryRepo = new Mock<IProductCategoryRepository>();
            mockCategoryRepo.Setup(r => r.GetAllCategories()).Returns(expected);
            Mock<ObjectCache> mockCache = new Mock<ObjectCache>();
            InventoryService sut = new InventoryService(null, mockCategoryRepo.Object, null, mockCache.Object, null);

            // act
            var actual = sut.GetCategories();

            // assert
            mockCache.Verify(
                c => c.Set("allCategories", It.Is<IEnumerable<ProductCategory>>(l => l.Count() == 3),
                           It.IsAny<CacheItemPolicy>(), null),
                Times.Once());
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        [TestCategory("BloomSales.Services.Tests.InventoryServiceTests")]
        public void GetInventoryByCity_ResultExistsInCache_ReturnsTheResult()
        {
            // arrange
            List<InventoryItem> expected = new List<InventoryItem>();
            expected.Add(new InventoryItem() { ID = 1 });
            expected.Add(new InventoryItem() { ID = 2 });
            expected.Add(new InventoryItem() { ID = 3 });
            expected.Add(new InventoryItem() { ID = 4 });
            Mock<ObjectCache> mockCache = new Mock<ObjectCache>();
            mockCache.Setup(c => c["inventoryInVancouver"]).Returns(expected);
            InventoryService sut = new InventoryService(null, null, null, mockCache.Object, null);

            // act
            var actual = sut.GetInventoryByCity("Vancouver");

            // assert
            Assert.IsTrue(Equality.AreEqual(expected, actual));
            mockCache.Verify(c => c["inventoryInVancouver"], Times.Once());
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        [TestCategory("BloomSales.Services.Tests.InventoryServiceTests")]
        public void GetInventoryByCity_ResultNotExistsInCache_FetchesTheResultFromDatabase()
        {
            // arrange
            List<InventoryItem> expected = new List<InventoryItem>();
            expected.Add(new InventoryItem() { ID = 1 });
            expected.Add(new InventoryItem() { ID = 2 });
            expected.Add(new InventoryItem() { ID = 3 });
            expected.Add(new InventoryItem() { ID = 4 });
            List<Warehouse> warehouses = new List<Warehouse>();
            warehouses.Add(new Warehouse() { ID = 10 });
            warehouses.Add(new Warehouse() { ID = 11 });
            warehouses.Add(new Warehouse() { ID = 12 });
            Mock<ILocationService> mockLocationService = new Mock<ILocationService>();
            mockLocationService.Setup(s => s.GetWarehousesByCity("Vancouver")).Returns(warehouses);
            Mock<IInventoryItemRepository> mockInventoryRepo = new Mock<IInventoryItemRepository>();
            mockInventoryRepo.Setup(r => r.GetInventories(It.Is<IEnumerable<Warehouse>>(l => l.Count() == 3)))
                .Returns(expected);
            Mock<ObjectCache> mockCache = new Mock<ObjectCache>();
            InventoryService sut = new InventoryService(mockInventoryRepo.Object, null, null,
                mockCache.Object, mockLocationService.Object);

            // act
            var actual = sut.GetInventoryByCity("Vancouver");

            // assert
            Assert.IsTrue(Equality.AreEqual(expected, actual));
            mockLocationService.Verify(s => s.GetWarehousesByCity("Vancouver"), Times.Once());
            mockInventoryRepo.Verify(r => r.GetInventories(It.Is<IEnumerable<Warehouse>>(l => l.Count() == 3)),
                Times.Once());
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        [TestCategory("BloomSales.Services.Tests.InventoryServiceTests")]
        public void GetInventoryByCity_AfterFetchingResultFromDatabase_CachesTheResult()
        {
            // arrange
            List<InventoryItem> expected = new List<InventoryItem>();
            expected.Add(new InventoryItem() { ID = 1 });
            expected.Add(new InventoryItem() { ID = 2 });
            expected.Add(new InventoryItem() { ID = 3 });
            expected.Add(new InventoryItem() { ID = 4 });
            List<Warehouse> warehouses = new List<Warehouse>();
            warehouses.Add(new Warehouse() { ID = 10 });
            warehouses.Add(new Warehouse() { ID = 11 });
            warehouses.Add(new Warehouse() { ID = 12 });
            Mock<ILocationService> mockLocationService = new Mock<ILocationService>();
            mockLocationService.Setup(s => s.GetWarehousesByCity("Vancouver")).Returns(warehouses);
            Mock<IInventoryItemRepository> mockInventoryRepo = new Mock<IInventoryItemRepository>();
            mockInventoryRepo.Setup(r => r.GetInventories(It.Is<IEnumerable<Warehouse>>(l => l.Count() == 3)))
                .Returns(expected);
            Mock<ObjectCache> mockCache = new Mock<ObjectCache>();
            InventoryService sut = new InventoryService(mockInventoryRepo.Object, null, null,
                mockCache.Object, mockLocationService.Object);

            // act
            var actual = sut.GetInventoryByCity("Vancouver");

            // assert
            mockCache.Verify(
                c => c.Set("inventoryInVancouver", It.Is<IEnumerable<InventoryItem>>(l => l.Count() == 4),
                           It.IsAny<CacheItemPolicy>(), null),
                Times.Once());
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        [TestCategory("BloomSales.Services.Tests.InventoryServiceTests")]
        public void GetInventoryByWarehouse_ResultExistsInCache_ReturnsResult()
        {
            // arrange
            List<InventoryItem> expected = new List<InventoryItem>();
            expected.Add(new InventoryItem() { ID = 1 });
            expected.Add(new InventoryItem() { ID = 2 });
            expected.Add(new InventoryItem() { ID = 3 });
            expected.Add(new InventoryItem() { ID = 4 });
            expected.Add(new InventoryItem() { ID = 5 });
            Mock<ObjectCache> mockCache = new Mock<ObjectCache>();
            mockCache.Setup(c => c["inventoryInW-12Warehouse"]).Returns(expected);
            InventoryService sut = new InventoryService(null, null, null, mockCache.Object, null);

            // act
            var actual = sut.GetInventoryByWarehouse("W-12");

            // assert
            Assert.IsTrue(Equality.AreEqual(expected, actual));
            mockCache.Verify(c => c["inventoryInW-12Warehouse"], Times.Once());
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        [TestCategory("BloomSales.Services.Tests.InventoryServiceTests")]
        public void GetInventoryByWarehouse_ResultNotExistsInCache_FetchesTheResultFromDatabase()
        {
            // arrange
            List<InventoryItem> expected = new List<InventoryItem>();
            expected.Add(new InventoryItem() { ID = 1 });
            expected.Add(new InventoryItem() { ID = 2 });
            expected.Add(new InventoryItem() { ID = 3 });
            Warehouse warehouse = new Warehouse() { ID = 13 };
            Mock<ObjectCache> mockCache = new Mock<ObjectCache>();
            Mock<ILocationService> mockLocationService = new Mock<ILocationService>();
            mockLocationService.Setup(s => s.GetWarehouseByName("W-13")).Returns(warehouse);
            Mock<IInventoryItemRepository> mockInventoryRepo = new Mock<IInventoryItemRepository>();
            mockInventoryRepo.Setup(r => r.GetInventory(It.Is<Warehouse>(w => w.ID == 13))).Returns(expected);
            InventoryService sut = new InventoryService(mockInventoryRepo.Object, null, null,
                mockCache.Object, mockLocationService.Object);

            // act
            var actual = sut.GetInventoryByWarehouse("W-13");

            // assert
            Assert.IsTrue(Equality.AreEqual(expected, actual));
            mockLocationService.Verify(s => s.GetWarehouseByName("W-13"), Times.Once());
            mockInventoryRepo.Verify(r => r.GetInventory(It.Is<Warehouse>(w => w.ID == 13)), Times.Once());
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        [TestCategory("BloomSales.Services.Tests.InventoryServiceTests")]
        public void GetInventoryByWarehouse_AfterFetchingResultFromDatabase_CachesTheResult()
        {
            // arrange
            List<InventoryItem> expected = new List<InventoryItem>();
            expected.Add(new InventoryItem() { ID = 1 });
            expected.Add(new InventoryItem() { ID = 2 });
            expected.Add(new InventoryItem() { ID = 3 });
            Warehouse warehouse = new Warehouse() { ID = 13 };
            Mock<ObjectCache> mockCache = new Mock<ObjectCache>();
            Mock<ILocationService> mockLocationService = new Mock<ILocationService>();
            mockLocationService.Setup(s => s.GetWarehouseByName("W-13")).Returns(warehouse);
            Mock<IInventoryItemRepository> mockInventoryRepo = new Mock<IInventoryItemRepository>();
            mockInventoryRepo.Setup(r => r.GetInventory(It.Is<Warehouse>(w => w.ID == 13))).Returns(expected);
            InventoryService sut = new InventoryService(mockInventoryRepo.Object, null, null,
                mockCache.Object, mockLocationService.Object);

            // act
            var actual = sut.GetInventoryByWarehouse("W-13");

            // assert
            mockCache.Verify(
                c => c.Set("inventoryInW-13Warehouse", It.Is<IEnumerable<InventoryItem>>(l => l.Count() == 3),
                           It.IsAny<CacheItemPolicy>(), null),
                Times.Once());
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        [TestCategory("BloomSales.Services.Tests.InventoryServiceTests")]
        public void GetInventoryByRegion_ResultExistsInCache_ReturnsTheResult()
        {
            // arrange
            List<InventoryItem> expected = new List<InventoryItem>();
            expected.Add(new InventoryItem() { ID = 1 });
            expected.Add(new InventoryItem() { ID = 2 });
            expected.Add(new InventoryItem() { ID = 3 });
            expected.Add(new InventoryItem() { ID = 4 });
            expected.Add(new InventoryItem() { ID = 5 });
            Mock<ObjectCache> mockCache = new Mock<ObjectCache>();
            mockCache.Setup(c => c[It.Is<string>(s => s.Equals("inventoryInEasternCanadaRegion"))])
                .Returns(expected);
            InventoryService sut = new InventoryService(null, null, null, mockCache.Object, null);

            // act
            var actual = sut.GetInventoryByRegion("EasternCanada");

            // assert
            Assert.IsTrue(Equality.AreEqual(expected, actual));
            mockCache.Verify(c => c[It.Is<string>(s => s.Equals("inventoryInEasternCanadaRegion"))],
                Times.Once());
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        [TestCategory("BloomSales.Services.Tests.InventoryServiceTests")]
        public void GetInventoryByRegion_ResultNotExistsInCache_FetchesTheResultFromDatabase()
        {
            // arrange
            List<InventoryItem> expected = new List<InventoryItem>();
            expected.Add(new InventoryItem() { ID = 1 });
            expected.Add(new InventoryItem() { ID = 2 });
            expected.Add(new InventoryItem() { ID = 3 });
            List<Warehouse> warehouses = new List<Warehouse>();
            warehouses.Add(new Warehouse() { ID = 33 });
            warehouses.Add(new Warehouse() { ID = 22 });
            Mock<ObjectCache> mockCache = new Mock<ObjectCache>();
            Mock<ILocationService> mockLocationService = new Mock<ILocationService>();
            mockLocationService.Setup(s => s.GetWarehousesByRegion("WesternCanada")).Returns(warehouses);
            Mock<IInventoryItemRepository> mockInventoryRepo = new Mock<IInventoryItemRepository>();
            mockInventoryRepo.Setup(r => r.GetInventories(It.Is<IEnumerable<Warehouse>>(l => l.Count() == 2)))
                .Returns(expected);
            InventoryService sut = new InventoryService(mockInventoryRepo.Object, null, null,
                mockCache.Object, mockLocationService.Object);

            // act
            var actual = sut.GetInventoryByRegion("WesternCanada");

            // assert
            Assert.IsTrue(Equality.AreEqual(expected, actual));
            mockLocationService.Verify(s => s.GetWarehousesByRegion("WesternCanada"), Times.Once());
            mockInventoryRepo.Verify(r => r.GetInventories(It.Is<IEnumerable<Warehouse>>(l => l.Count() == 2)),
                Times.Once());
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        [TestCategory("BloomSales.Services.Tests.InventoryServiceTests")]
        public void GetInventoryByRegion_AfterFetchingResultFromDatabase_CachesTheResult()
        {
            // arrange
            List<InventoryItem> expected = new List<InventoryItem>();
            expected.Add(new InventoryItem() { ID = 1 });
            expected.Add(new InventoryItem() { ID = 2 });
            expected.Add(new InventoryItem() { ID = 3 });
            List<Warehouse> warehouses = new List<Warehouse>();
            warehouses.Add(new Warehouse() { ID = 33 });
            warehouses.Add(new Warehouse() { ID = 22 });
            Mock<ObjectCache> mockCache = new Mock<ObjectCache>();
            Mock<ILocationService> mockLocationService = new Mock<ILocationService>();
            mockLocationService.Setup(s => s.GetWarehousesByRegion("WesternCanada")).Returns(warehouses);
            Mock<IInventoryItemRepository> mockInventoryRepo = new Mock<IInventoryItemRepository>();
            mockInventoryRepo.Setup(r => r.GetInventories(It.Is<IEnumerable<Warehouse>>(l => l.Count() == 2)))
                .Returns(expected);
            InventoryService sut = new InventoryService(mockInventoryRepo.Object, null, null,
                mockCache.Object, mockLocationService.Object);

            // act
            var actual = sut.GetInventoryByRegion("WesternCanada");

            // assert
            mockCache.Verify(
                c => c.Set("inventoryInWesternCanadaRegion",
                           It.Is<IEnumerable<InventoryItem>>(l => l.Count() == 3),
                           It.IsAny<CacheItemPolicy>(), null),
                Times.Once());
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        [TestCategory("BloomSales.Services.Tests.InventoryServiceTests")]
        public void GetStocksByCity_ResultExistsInCache_ReturnsTheResult()
        {
            // arrange
            List<InventoryItem> expected = new List<InventoryItem>();
            expected.Add(new InventoryItem() { UnitsInStock = 12 });
            expected.Add(new InventoryItem() { UnitsInStock = 14 });
            expected.Add(new InventoryItem() { UnitsInStock = 13 });
            Mock<ObjectCache> mockCache = new Mock<ObjectCache>();
            mockCache.Setup(c => c["stocksOfP100InToronto"]).Returns(expected);
            InventoryService sut = new InventoryService(null, null, null, mockCache.Object, null);

            // act
            var actual = sut.GetStocksByCity("Toronto", 100);

            // assert
            Assert.IsTrue(Equality.AreEqual(expected, actual));
            mockCache.Verify(c => c["stocksOfP100InToronto"], Times.Once());
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        [TestCategory("BloomSales.Services.Tests.InventoryServiceTests")]
        public void GetStocksByCity_ResultNotExistsInCache_FetchesTheResultFromDatabase()
        {
            // arrange
            List<InventoryItem> expected = new List<InventoryItem>();
            expected.Add(new InventoryItem() { UnitsInStock = 120 });
            expected.Add(new InventoryItem() { UnitsInStock = 140 });
            expected.Add(new InventoryItem() { UnitsInStock = 130 });
            expected.Add(new InventoryItem() { UnitsInStock = 150 });
            List<Warehouse> warehouses = new List<Warehouse>();
            warehouses.Add(new Warehouse() { ID = 12 });
            warehouses.Add(new Warehouse() { ID = 15 });
            warehouses.Add(new Warehouse() { ID = 17 });
            Mock<ObjectCache> mockCache = new Mock<ObjectCache>();
            Mock<ILocationService> mockLocationService = new Mock<ILocationService>();
            mockLocationService.Setup(s => s.GetWarehousesByCity("Calgary")).Returns(warehouses);
            Mock<IInventoryItemRepository> mockInventoryRepo = new Mock<IInventoryItemRepository>();
            mockInventoryRepo.Setup(
                r => r.GetStocks(It.Is<IEnumerable<Warehouse>>(l => l.Count() == 3), 200)).Returns(expected);
            InventoryService sut = new InventoryService(mockInventoryRepo.Object, null, null,
                mockCache.Object, mockLocationService.Object);

            // act
            var actual = sut.GetStocksByCity("Calgary", 200);

            // assert
            Assert.IsTrue(Equality.AreEqual(expected, actual));
            mockLocationService.Verify(s => s.GetWarehousesByCity("Calgary"), Times.Once());
            mockInventoryRepo.Verify(
                r => r.GetStocks(It.Is<IEnumerable<Warehouse>>(l => l.Count() == 3), 200), Times.Once());
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        [TestCategory("BloomSales.Services.Tests.InventoryServiceTests")]
        public void GetStocksByCity_AfterFetchingResultFromDatabase_CachesTheResult()
        {
            // arrange
            List<InventoryItem> expected = new List<InventoryItem>();
            expected.Add(new InventoryItem() { UnitsInStock = 120 });
            expected.Add(new InventoryItem() { UnitsInStock = 140 });
            expected.Add(new InventoryItem() { UnitsInStock = 130 });
            expected.Add(new InventoryItem() { UnitsInStock = 150 });
            List<Warehouse> warehouses = new List<Warehouse>();
            warehouses.Add(new Warehouse() { ID = 12 });
            warehouses.Add(new Warehouse() { ID = 15 });
            warehouses.Add(new Warehouse() { ID = 17 });
            Mock<ObjectCache> mockCache = new Mock<ObjectCache>();
            Mock<ILocationService> mockLocationService = new Mock<ILocationService>();
            mockLocationService.Setup(s => s.GetWarehousesByCity("Calgary")).Returns(warehouses);
            Mock<IInventoryItemRepository> mockInventoryRepo = new Mock<IInventoryItemRepository>();
            mockInventoryRepo.Setup(
                r => r.GetStocks(It.Is<IEnumerable<Warehouse>>(l => l.Count() == 3), 200)).Returns(expected);
            InventoryService sut = new InventoryService(mockInventoryRepo.Object, null, null,
                mockCache.Object, mockLocationService.Object);

            // act
            var actual = sut.GetStocksByCity("Calgary", 200);

            // assert
            mockCache.Verify(
                c => c.Set("stocksOfP200InCalgary", It.Is<IEnumerable<InventoryItem>>(l => l.Count() == 4),
                           It.IsAny<CacheItemPolicy>(), null),
                Times.Once());
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        [TestCategory("BloomSales.Services.Tests.InventoryServiceTests")]
        public void GetStocksByRegion_ResultExistsInCache_ReturnsTheResult()
        {
            // arrange
            List<InventoryItem> expected = new List<InventoryItem>();
            expected.Add(new InventoryItem() { UnitsInStock = 120 });
            expected.Add(new InventoryItem() { UnitsInStock = 140 });
            expected.Add(new InventoryItem() { UnitsInStock = 130 });
            expected.Add(new InventoryItem() { UnitsInStock = 150 });
            Mock<ObjectCache> mockCache = new Mock<ObjectCache>();
            mockCache.Setup(c => c["stocksOfP300InEasternCanadaRegion"]).Returns(expected);
            InventoryService sut = new InventoryService(null, null, null, mockCache.Object, null);

            // act
            var actual = sut.GetStocksByRegion("EasternCanada", 300);

            // assert
            Assert.IsTrue(Equality.AreEqual(expected, actual));
            mockCache.Verify(c => c["stocksOfP300InEasternCanadaRegion"], Times.Once());
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        [TestCategory("BloomSales.Services.Tests.InventoryServiceTests")]
        public void GetStocksByRegion_ResultNotExistsCache_FetchesTheResultFromDatabase()
        {
            // arrange
            List<InventoryItem> expected = new List<InventoryItem>();
            expected.Add(new InventoryItem() { UnitsInStock = 20 });
            expected.Add(new InventoryItem() { UnitsInStock = 40 });
            expected.Add(new InventoryItem() { UnitsInStock = 30 });
            expected.Add(new InventoryItem() { UnitsInStock = 50 });
            List<Warehouse> warehouses = new List<Warehouse>();
            warehouses.Add(new Warehouse() { ID = 1 });
            warehouses.Add(new Warehouse() { ID = 2 });
            Mock<ObjectCache> mockCache = new Mock<ObjectCache>();
            Mock<ILocationService> mockLocationService = new Mock<ILocationService>();
            mockLocationService.Setup(s => s.GetWarehousesByRegion("WesternCanada")).Returns(warehouses);
            Mock<IInventoryItemRepository> mockInventoryRepo = new Mock<IInventoryItemRepository>();
            mockInventoryRepo.Setup(r => r.GetStocks(It.Is<IEnumerable<Warehouse>>(l => l.Count() == 2), 300))
                .Returns(expected);
            InventoryService sut = new InventoryService(mockInventoryRepo.Object, null, null,
                mockCache.Object, mockLocationService.Object);

            // act
            var actual = sut.GetStocksByRegion("WesternCanada", 300);

            // assert
            Assert.IsTrue(Equality.AreEqual(expected, actual));
            mockLocationService.Verify(s => s.GetWarehousesByRegion("WesternCanada"), Times.Once());
            mockInventoryRepo.Verify(r => r.GetStocks(It.Is<IEnumerable<Warehouse>>(l => l.Count() == 2), 300),
                Times.Once());
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        [TestCategory("BloomSales.Services.Tests.InventoryServiceTests")]
        public void GetStocksByRegion_AfterFetchingResultFromDatabase_CachesTheResult()
        {
            // arrange
            List<InventoryItem> expected = new List<InventoryItem>();
            expected.Add(new InventoryItem() { UnitsInStock = 20 });
            expected.Add(new InventoryItem() { UnitsInStock = 40 });
            expected.Add(new InventoryItem() { UnitsInStock = 30 });
            expected.Add(new InventoryItem() { UnitsInStock = 50 });
            List<Warehouse> warehouses = new List<Warehouse>();
            warehouses.Add(new Warehouse() { ID = 1 });
            warehouses.Add(new Warehouse() { ID = 2 });
            Mock<ObjectCache> mockCache = new Mock<ObjectCache>();
            Mock<ILocationService> mockLocationService = new Mock<ILocationService>();
            mockLocationService.Setup(s => s.GetWarehousesByRegion("WesternCanada")).Returns(warehouses);
            Mock<IInventoryItemRepository> mockInventoryRepo = new Mock<IInventoryItemRepository>();
            mockInventoryRepo.Setup(r => r.GetStocks(It.Is<IEnumerable<Warehouse>>(l => l.Count() == 2), 300))
                .Returns(expected);
            InventoryService sut = new InventoryService(mockInventoryRepo.Object, null, null,
                mockCache.Object, mockLocationService.Object);

            // act
            var actual = sut.GetStocksByRegion("WesternCanada", 300);

            // assert
            mockCache.Verify(
                c => c.Set("stocksOfP300InWesternCanadaRegion",
                           It.Is<IEnumerable<InventoryItem>>(l => l.Count() == 4),
                           It.IsAny<CacheItemPolicy>(),
                           null),
                Times.Once());
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        [TestCategory("BloomSales.Services.Tests.InventoryServiceTests")]
        public void GetStockByWarehouse_ResultExistsInCache_ReturnsTheResult()
        {
            // arrange
            InventoryItem expected = new InventoryItem() { ID = 11 };
            Mock<ObjectCache> mockCache = new Mock<ObjectCache>();
            mockCache.Setup(c => c["stockOfP700InW12Warehouse"]).Returns(expected);
            InventoryService sut = new InventoryService(null, null, null, mockCache.Object, null);

            // act
            var actual = sut.GetStockByWarehouse("W12", 700);

            // assert
            Assert.AreEqual(expected, actual);
            mockCache.Verify(c => c["stockOfP700InW12Warehouse"], Times.Once());
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        [TestCategory("BloomSales.Services.Tests.InventoryServiceTests")]
        public void GetStockByWarehouse_ResultNotExistsInCache_FetchesTheResultFromDatabase()
        {
            // arrange
            InventoryItem expected = new InventoryItem() { ID = 21 };
            Warehouse warehouse = new Warehouse() { ID = 3, Name = "W3" };
            Mock<ObjectCache> mockCache = new Mock<ObjectCache>();
            Mock<ILocationService> mockLocationService = new Mock<ILocationService>();
            mockLocationService.Setup(s => s.GetWarehouseByName("W3")).Returns(warehouse);
            Mock<IInventoryItemRepository> mockInventoryRepo = new Mock<IInventoryItemRepository>();
            mockInventoryRepo.Setup(r => r.GetStock(It.Is<Warehouse>(w => w.ID == 3), 100))
                .Returns(expected);
            InventoryService sut = new InventoryService(mockInventoryRepo.Object, null, null,
                mockCache.Object, mockLocationService.Object);

            // act
            var actual = sut.GetStockByWarehouse("W3", 100);

            // assert
            Assert.AreEqual(expected, actual);
            mockLocationService.Verify(s => s.GetWarehouseByName("W3"), Times.Once());
            mockInventoryRepo.Verify(r => r.GetStock(It.Is<Warehouse>(w => w.ID == 3), 100), Times.Once());
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        [TestCategory("BloomSales.Services.Tests.InventoryServiceTests")]
        public void GetStockByWarehouse_AfterFetchingResultFromDatabase_CachesTheResult()
        {
            // arrange
            InventoryItem expected = new InventoryItem() { ID = 21 };
            Warehouse warehouse = new Warehouse() { ID = 3, Name = "W3" };
            Mock<ObjectCache> mockCache = new Mock<ObjectCache>();
            Mock<ILocationService> mockLocationService = new Mock<ILocationService>();
            mockLocationService.Setup(s => s.GetWarehouseByName("W3")).Returns(warehouse);
            Mock<IInventoryItemRepository> mockInventoryRepo = new Mock<IInventoryItemRepository>();
            mockInventoryRepo.Setup(r => r.GetStock(It.Is<Warehouse>(w => w.ID == 3), 100))
                .Returns(expected);
            InventoryService sut = new InventoryService(mockInventoryRepo.Object, null, null,
                mockCache.Object, mockLocationService.Object);

            // act
            var actual = sut.GetStockByWarehouse("W3", 100);

            // assert
            mockCache.Verify(
                c => c.Set("stockOfP100InW3Warehouse", It.Is<InventoryItem>(i => i.ID == 21),
                           It.IsAny<CacheItemPolicy>(), null),
                Times.Once());
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        [TestCategory("BloomSales.Services.Tests.InventoryServiceTests")]
        public void AddToInventory_GivenANewItem_AddsToDatabase()
        {
            // arrange
            InventoryItem item = new InventoryItem() { ID = 100 };
            Mock<IInventoryItemRepository> mockInventoryRepo = new Mock<IInventoryItemRepository>();
            InventoryService sut = new InventoryService(mockInventoryRepo.Object, null, null, null, null);

            // act
            sut.AddToInventory(item);

            // assert
            mockInventoryRepo.Verify(r => r.AddToInventory(It.Is<InventoryItem>(i => i.ID == 100)), Times.Once());
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        [TestCategory("BloomSales.Services.Tests.InventoryServiceTests")]
        public void UpdateInventory_GivenAValidItemIDAndNewStockValue_UpdatesTheRecordInDatabase()
        {
            // arrange
            Mock<IInventoryItemRepository> mockInventoryRepo = new Mock<IInventoryItemRepository>();
            InventoryService sut = new InventoryService(mockInventoryRepo.Object, null, null, null, null);

            // act
            sut.UpdateStock(20, 140);

            // assert
            mockInventoryRepo.Verify(r => r.UpdateStock(20, 140), Times.Once());
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        [TestCategory("BloomSales.Services.Tests.InventoryServiceTests")]
        public void AddCategory_GivenANewCategory_AddsToDatabase()
        {
            // arrange
            ProductCategory category = new ProductCategory() { Name = "Category1" };
            Mock<IProductCategoryRepository> mockCategoryRepo = new Mock<IProductCategoryRepository>();
            InventoryService sut = new InventoryService(null, mockCategoryRepo.Object, null, null, null);

            // act
            sut.AddCategory(category);

            // assert
            mockCategoryRepo.Verify(
                r => r.AddCategory(It.Is<ProductCategory>(c => c.Name.Equals("Category1"))),
                Times.Once());
        }
    }
}