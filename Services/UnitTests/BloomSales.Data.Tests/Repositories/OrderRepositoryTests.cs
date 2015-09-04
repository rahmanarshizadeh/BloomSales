using BloomSales.Data.Entities;
using BloomSales.Data.Repositories;
using BloomSales.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloomSales.Data.Tests.Repositories
{
    [TestClass]
    public class OrderRepositoryTests
    {
        [TestMethod]
        public void AddOrder_GivenANewOrder_AddsToDatabase()
        {
            // arrange
            List<Order> data = new List<Order>();
            Order order = new Order() { ID = 3 };
            Mock<DbSet<Order>> mockSet = new Mock<DbSet<Order>>();
            Mock<OrdersDb> mockContext = new Mock<OrdersDb>();
            mockContext.Setup(c => c.Orders).Returns(mockSet.Object);
            OrderRepository sut = new OrderRepository(mockContext.Object);

            // act
            sut.AddOrder(order);

            // assert
            mockSet.Verify(s => s.Add(It.Is<Order>(o => o.ID == 3)), Times.Once());
            mockContext.Verify(c => c.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void GetOrder_GivenAValidID_ReturnsTheOrderRecord()
        {
            // arrange
            List<Order> data = new List<Order>();
            data.Add(new Order() { ID = 1 });
            data.Add(new Order() { ID = 3 });
            data.Add(new Order() { ID = 5 });
            data.Add(new Order() { ID = 7 });
            Order expected = data[2];
            Mock<DbSet<Order>> mockSet = new Mock<DbSet<Order>>();
            mockSet.Setup(s => s.Find(5)).Returns(expected);
            Mock<OrdersDb> mockContext = new Mock<OrdersDb>();
            mockContext.Setup(c => c.Orders).Returns(mockSet.Object);
            OrderRepository sut = new OrderRepository(mockContext.Object);

            // act
            var actual = sut.GetOrder(5);

            // assert
            Assert.AreEqual(expected, actual);
            mockSet.Verify(s => s.Find(5), Times.Once());
        }

        [TestMethod]
        public void GetOrdersByCustomer_GivenAValidCustomerIDAndAPeriodOfTime_ReturnsAppropriateRecords()
        {
            // arrange
            List<Order> data = new List<Order>();
            data.Add(new Order() { CustomerID = 12, OrderDate = new DateTime(2015, 4, 1) });
            data.Add(new Order() { CustomerID = 13, OrderDate = new DateTime(2015, 4, 1) });
            data.Add(new Order() { CustomerID = 12, OrderDate = new DateTime(2015, 5, 1) });
            data.Add(new Order() { CustomerID = 12, OrderDate = new DateTime(2015, 6, 1) });
            data.Add(new Order() { CustomerID = 12, OrderDate = new DateTime(2015, 7, 1) });
            data.Add(new Order() { CustomerID = 15, OrderDate = new DateTime(2015, 4, 1) });
            data.Add(new Order() { CustomerID = 16, OrderDate = new DateTime(2015, 4, 1) });
            data.Add(new Order() { CustomerID = 17, OrderDate = new DateTime(2015, 4, 1) });
            data.Add(new Order() { CustomerID = 12, OrderDate = new DateTime(2015, 8, 1) });
            List<Order> expected = new List<Order>();
            expected.Add(data[0]);
            expected.Add(data[2]);
            expected.Add(data[3]);
            expected.Add(data[4]);
            expected.Add(data[8]);
            Mock<DbSet<Order>> mockSet = EntityMockFactory.CreateSet(data.AsQueryable());
            Mock<OrdersDb> mockContext = new Mock<OrdersDb>();
            mockContext.Setup(c => c.Orders).Returns(mockSet.Object);
            OrderRepository sut = new OrderRepository(mockContext.Object);

            // act
            var actual = sut.GetOrdersByCustomer(12, new DateTime(2015, 1, 1), new DateTime(2016, 1, 1));

            // assert
            Assert.IsTrue(Equality.AreEqual(expected, actual));
        }
    }
}
