using BloomSales.Data.Entities;
using BloomSales.Data.Repositories;
using BloomSales.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace BloomSales.Data.Tests.Repositories
{
    [TestClass]
    public class InventoryItemRepositoryTests
    {
        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        public void GetInventories_GivenAValidListOfWarehouses_ReturnsTheirInventories()
        {
            List<Warehouse> warehouses = new List<Warehouse>();
            warehouses.Add(new Warehouse() { ID = 21 });
            warehouses.Add(new Warehouse() { ID = 22 });
            warehouses.Add(new Warehouse() { ID = 23 });
            List<InventoryItem> data = new List<InventoryItem>();
            data.Add(new InventoryItem() { ID = 1, WarehouseID = 20 });
            data.Add(new InventoryItem() { ID = 1, WarehouseID = 21 });
            data.Add(new InventoryItem() { ID = 1, WarehouseID = 21 });
            data.Add(new InventoryItem() { ID = 1, WarehouseID = 22 });
            data.Add(new InventoryItem() { ID = 1, WarehouseID = 23 });
            data.Add(new InventoryItem() { ID = 1, WarehouseID = 24 });
            data.Add(new InventoryItem() { ID = 1, WarehouseID = 24 });
            data.Add(new InventoryItem() { ID = 1, WarehouseID = 25 });
            List<InventoryItem> expected = new List<InventoryItem>();
            expected.Add(data[1]);
            expected.Add(data[2]);
            expected.Add(data[3]);
            expected.Add(data[4]);
            Mock<DbSet<InventoryItem>> mockSet = EntityMockFactory.CreateSet(data.AsQueryable());
            Mock<InventoryDb> mockContext = new Mock<InventoryDb>();
            mockContext.Setup(c => c.Inventories).Returns(mockSet.Object);
            InventoryItemRepository sut = new InventoryItemRepository(mockContext.Object);

            // act
            var actual = sut.GetInventories(warehouses);

            // assert
            Assert.IsTrue(Equality.AreEqual(expected, actual));
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        public void GetInventory_GivenAValidWarehoue_ReturnsItsInventory()
        {
            Warehouse warehouse = new Warehouse() { ID = 11 };
            List<InventoryItem> data = new List<InventoryItem>();
            data.Add(new InventoryItem() { ID = 1, WarehouseID = 20 });
            data.Add(new InventoryItem() { ID = 1, WarehouseID = 21 });
            data.Add(new InventoryItem() { ID = 1, WarehouseID = 21 });
            data.Add(new InventoryItem() { ID = 1, WarehouseID = 11 });
            data.Add(new InventoryItem() { ID = 1, WarehouseID = 23 });
            data.Add(new InventoryItem() { ID = 1, WarehouseID = 11 });
            data.Add(new InventoryItem() { ID = 1, WarehouseID = 11 });
            data.Add(new InventoryItem() { ID = 1, WarehouseID = 25 });
            List<InventoryItem> expected = new List<InventoryItem>();
            expected.Add(data[3]);
            expected.Add(data[5]);
            expected.Add(data[6]);
            Mock<DbSet<InventoryItem>> mockSet = EntityMockFactory.CreateSet(data.AsQueryable());
            Mock<InventoryDb> mockContext = new Mock<InventoryDb>();
            mockContext.Setup(c => c.Inventories).Returns(mockSet.Object);
            InventoryItemRepository sut = new InventoryItemRepository(mockContext.Object);

            // act
            var actual = sut.GetInventory(warehouse);

            // assert
            Assert.IsTrue(Equality.AreEqual(expected, actual));
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        public void GetStock_GivenAValidWarehouseAndProductID_ReturnsItsStockInTheWarehouse()
        {
            // arrange
            Warehouse warehouse = new Warehouse() { ID = 15 };
            List<InventoryItem> data = new List<InventoryItem>();
            data.Add(new InventoryItem() { WarehouseID = 11, ProductID = 100 });
            data.Add(new InventoryItem() { WarehouseID = 15, ProductID = 101 });
            data.Add(new InventoryItem() { WarehouseID = 12, ProductID = 101 });
            data.Add(new InventoryItem() { WarehouseID = 15, ProductID = 130 });
            data.Add(new InventoryItem() { WarehouseID = 14, ProductID = 120 });
            data.Add(new InventoryItem() { WarehouseID = 15, ProductID = 150 });
            InventoryItem expected = data[5];
            Mock<DbSet<InventoryItem>> mockSet = EntityMockFactory.CreateSet(data.AsQueryable());
            Mock<InventoryDb> mockContext = new Mock<InventoryDb>();
            mockContext.Setup(c => c.Inventories).Returns(mockSet.Object);
            InventoryItemRepository sut = new InventoryItemRepository(mockContext.Object);

            // act
            var actual = sut.GetStock(warehouse, 150);

            // assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        public void GetStock_GivenAValidListOfWarehousesAndAProductID_ReturnsTheStocksInTheWarehouses()
        {
            // arrange
            List<Warehouse> warehouses = new List<Warehouse>();
            warehouses.Add(new Warehouse() { ID = 11 });
            warehouses.Add(new Warehouse() { ID = 15 });
            warehouses.Add(new Warehouse() { ID = 12 });
            List<InventoryItem> data = new List<InventoryItem>();
            data.Add(new InventoryItem() { WarehouseID = 11, ProductID = 101 });
            data.Add(new InventoryItem() { WarehouseID = 15, ProductID = 101 });
            data.Add(new InventoryItem() { WarehouseID = 12, ProductID = 101 });
            data.Add(new InventoryItem() { WarehouseID = 15, ProductID = 130 });
            data.Add(new InventoryItem() { WarehouseID = 14, ProductID = 120 });
            data.Add(new InventoryItem() { WarehouseID = 15, ProductID = 150 });
            List<InventoryItem> expected = new List<InventoryItem>();
            expected.Add(data[0]);
            expected.Add(data[1]);
            expected.Add(data[2]);
            Mock<DbSet<InventoryItem>> mockSet = EntityMockFactory.CreateSet(data.AsQueryable());
            Mock<InventoryDb> mockContext = new Mock<InventoryDb>();
            mockContext.Setup(c => c.Inventories).Returns(mockSet.Object);
            InventoryItemRepository sut = new InventoryItemRepository(mockContext.Object);

            // act
            var actual = sut.GetStocks(warehouses, 101);

            // assert
            Assert.IsTrue(Equality.AreEqual(expected, actual));
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        public void AddToInventory_GivenANewItem_AddsToDatabase()
        {
            // arrange
            InventoryItem item = new InventoryItem() { ID = 2 };
            List<InventoryItem> data = new List<InventoryItem>();
            Mock<DbSet<InventoryItem>> mockSet = new Mock<DbSet<InventoryItem>>();
            Mock<InventoryDb> mockContext = new Mock<InventoryDb>();
            mockContext.Setup(c => c.Inventories).Returns(mockSet.Object);
            InventoryItemRepository sut = new InventoryItemRepository(mockContext.Object);

            // act
            sut.AddToInventory(item);

            // assert
            mockSet.Verify(s => s.Add(It.Is<InventoryItem>(i => i.ID == 2)), Times.Once());
            mockContext.Verify(c => c.SaveChanges(), Times.Once());
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        public void UpdateStock_GivenAValidItemIDAndANewStockValue_UpdatesTheRecord()
        {
            // arrange
            List<InventoryItem> data = new List<InventoryItem>();
            data.Add(new InventoryItem() { ID = 12, UnitsInStock = 3 });
            data.Add(new InventoryItem() { ID = 20, UnitsInStock = 40 });
            data.Add(new InventoryItem() { ID = 18, UnitsInStock = 31 });
            Mock<DbSet<InventoryItem>> mockSet = EntityMockFactory.CreateSet(data.AsQueryable());
            Mock<InventoryDb> mockContext = new Mock<InventoryDb>();
            mockContext.Setup(c => c.Inventories).Returns(mockSet.Object);
            InventoryItemRepository sut = new InventoryItemRepository(mockContext.Object);

            // act
            sut.UpdateStock(20, 50);

            // assert
            Assert.AreEqual(50, data[1].UnitsInStock);
            mockContext.Verify(c => c.SaveChanges(), Times.Once());
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        public void UpdateStock_GivenAValidItemIDAndTheSameStockValue_IngnoresAndDoesNothing()
        {
            // arrange
            List<InventoryItem> data = new List<InventoryItem>();
            data.Add(new InventoryItem() { ID = 1, UnitsInStock = 1 });
            data.Add(new InventoryItem() { ID = 2, UnitsInStock = 12 });
            data.Add(new InventoryItem() { ID = 21, UnitsInStock = 50 });
            data.Add(new InventoryItem() { ID = 18, UnitsInStock = 31 });
            Mock<DbSet<InventoryItem>> mockSet = EntityMockFactory.CreateSet(data.AsQueryable());
            Mock<InventoryDb> mockContext = new Mock<InventoryDb>();
            mockContext.Setup(c => c.Inventories).Returns(mockSet.Object);
            InventoryItemRepository sut = new InventoryItemRepository(mockContext.Object);

            // act
            sut.UpdateStock(21, 50);

            // assert
            Assert.AreEqual(50, data[2].UnitsInStock);
            mockContext.Verify(c => c.SaveChanges(), Times.Never());
        }
    }
}