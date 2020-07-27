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
    public class OrderItemRepositoryTests
    {
        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        public void AddItem_GivenANewItem_AddsToDatabase()
        {
            // arrange
            OrderItem item = new OrderItem() { ID = 23 };
            Mock<DbSet<OrderItem>> mockSet = new Mock<DbSet<OrderItem>>();
            Mock<OrderDb> mockContext = new Mock<OrderDb>();
            mockContext.Setup(c => c.OrderItems).Returns(mockSet.Object);
            OrderItemRepository sut = new OrderItemRepository(mockContext.Object);

            // act
            sut.AddItem(item);

            // assert
            mockSet.Verify(s => s.Add(It.Is<OrderItem>(oi => oi.ID == 23)), Times.Once());
            mockContext.Verify(c => c.SaveChanges(), Times.Once());
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        public void AddItems_GivenAListOfNewItems_AddsThemToDatabase()
        {
            // arrange
            List<OrderItem> items = new List<OrderItem>();
            items.Add(new OrderItem() { ID = 30 });
            items.Add(new OrderItem() { ID = 31 });
            items.Add(new OrderItem() { ID = 32 });
            items.Add(new OrderItem() { ID = 33 });
            Mock<DbSet<OrderItem>> mockSet = new Mock<DbSet<OrderItem>>();
            Mock<OrderDb> mockContext = new Mock<OrderDb>();
            mockContext.Setup(c => c.OrderItems).Returns(mockSet.Object);
            OrderItemRepository sut = new OrderItemRepository(mockContext.Object);

            // act
            sut.AddItems(items);

            // assert
            mockSet.Verify(s => s.AddRange(It.Is<IEnumerable<OrderItem>>(l => l.Count() == 4)), Times.Once());
            mockContext.Verify(c => c.SaveChanges(), Times.Once());
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        public void GetItemsByOrder_GivenAValidOrderID_ReturnsTheListOfItemsForThatOrder()
        {
            // arrange
            List<OrderItem> data = new List<OrderItem>();
            data.Add(new OrderItem() { ID = 30, OrderID = 10 });
            data.Add(new OrderItem() { ID = 31, OrderID = 11 });
            data.Add(new OrderItem() { ID = 32, OrderID = 11 });
            data.Add(new OrderItem() { ID = 33, OrderID = 11 });
            data.Add(new OrderItem() { ID = 34, OrderID = 12 });
            data.Add(new OrderItem() { ID = 35, OrderID = 12 });
            List<OrderItem> expected = new List<OrderItem>();
            expected.Add(data[1]);
            expected.Add(data[2]);
            expected.Add(data[3]);
            Mock<DbSet<OrderItem>> mockSet = EntityMockFactory.CreateSet(data.AsQueryable());
            Mock<OrderDb> mockContext = new Mock<OrderDb>();
            mockContext.Setup(c => c.OrderItems).Returns(mockSet.Object);
            OrderItemRepository sut = new OrderItemRepository(mockContext.Object);

            // act
            var actual = sut.GetItemsByOrder(11);

            // assert
            Assert.IsTrue(Equality.AreEqual(expected, actual));
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        public void RemoveItem_GivenAValidID_DeletesTheRecordFromDatabase()
        {
            // arrange
            OrderItem item = new OrderItem() { ID = 10 };
            Mock<DbSet<OrderItem>> mockSet = new Mock<DbSet<OrderItem>>();
            mockSet.Setup(s => s.Find(10)).Returns(item);
            Mock<OrderDb> mockContext = new Mock<OrderDb>();
            mockContext.Setup(c => c.OrderItems).Returns(mockSet.Object);
            OrderItemRepository sut = new OrderItemRepository(mockContext.Object);

            // act
            sut.RemoveItem(10);

            // assert
            mockSet.Verify(s => s.Remove(It.Is<OrderItem>(oi => oi.ID == 10)), Times.Once());
            mockContext.Verify(c => c.SaveChanges(), Times.Once());
        }
    }
}