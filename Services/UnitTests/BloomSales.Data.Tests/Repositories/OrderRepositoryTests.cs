using BloomSales.Data.Entities;
using BloomSales.Data.Repositories;
using BloomSales.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace BloomSales.Data.Tests.Repositories
{
    [TestClass]
    public class OrderRepositoryTests
    {
        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        public void AddOrder_GivenANewOrder_AddsToDatabase()
        {
            // arrange
            List<Order> data = new List<Order>();
            Order order = new Order() { ID = 3 };
            Mock<DbSet<Order>> mockSet = new Mock<DbSet<Order>>();
            Mock<OrderDb> mockContext = new Mock<OrderDb>();
            mockContext.Setup(c => c.Orders).Returns(mockSet.Object);
            OrderRepository sut = new OrderRepository(mockContext.Object);

            // act
            sut.AddOrder(order);

            // assert
            mockSet.Verify(s => s.Add(It.Is<Order>(o => o.ID == 3)), Times.Once());
            mockContext.Verify(c => c.SaveChanges(), Times.Once());
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        public void GetOrder_GivenIdOfAnOrderHavingMultipleSuborders_ReturnsTheFullOrderObject()
        {
            // arrange
            List<Order> orderData = CreateOrderData();
            List<OrderItem> orderItemsData = CreateOrderItemsData();
            List<Order> expectedSubOrders = new List<Order>();
            expectedSubOrders.Add(orderData[3]);
            expectedSubOrders.Add(orderData[4]);
            List<OrderItem> expectedSubOrdersItems = new List<OrderItem>();
            expectedSubOrdersItems.Add(orderItemsData[4]);
            expectedSubOrdersItems.Add(orderItemsData[5]);
            expectedSubOrdersItems.Add(orderItemsData[6]);
            expectedSubOrdersItems.Add(orderItemsData[7]);
            Order expected = orderData[2];
            Mock<DbSet<Order>> mockOrdersSet = EntityMockFactory.CreateSet(orderData.AsQueryable());
            mockOrdersSet.Setup(s => s.Find(5)).Returns(orderData[2]);
            Mock<DbSet<OrderItem>> mockOrderItemsSet = EntityMockFactory.CreateSet(orderItemsData.AsQueryable());
            Mock<OrderDb> mockContext = new Mock<OrderDb>();
            mockContext.Setup(c => c.Orders).Returns(mockOrdersSet.Object);
            mockContext.Setup(c => c.OrderItems).Returns(mockOrderItemsSet.Object);
            OrderRepository sut = new OrderRepository(mockContext.Object);

            // act
            var actual = sut.GetOrder(5);

            // assert
            var actualSubOrders = new List<Order>(actual.SubOrders);
            List<OrderItem> actualSubOrderItems = new List<OrderItem>();
            foreach (Order o in actualSubOrders)
                foreach (OrderItem oi in o.Items)
                    actualSubOrderItems.Add(oi);
            Assert.AreEqual(expected, actual);
            Assert.IsTrue(Equality.AreEqual(expectedSubOrders, actualSubOrders));
            Assert.IsTrue(Equality.AreEqual(expectedSubOrdersItems, actualSubOrderItems));
            mockOrdersSet.Verify(s => s.Find(5), Times.Once());
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        public void GetOrdersByCustomer_GivenAValidCustomerIDAndAPeriodOfTime_ReturnsAppropriateRecords()
        {
            // arrange
            List<Order> data = new List<Order>();
            data.Add(new Order() { CustomerID = "12", OrderDate = new DateTime(2015, 4, 1), ParentOrderID = -1 });
            data.Add(new Order() { CustomerID = "13", OrderDate = new DateTime(2015, 4, 1), ParentOrderID = -1 });
            data.Add(new Order() { CustomerID = "12", OrderDate = new DateTime(2015, 5, 1), ParentOrderID = -1 });
            data.Add(new Order() { CustomerID = "12", OrderDate = new DateTime(2015, 6, 1), ParentOrderID = -1 });
            data.Add(new Order() { CustomerID = "12", OrderDate = new DateTime(2015, 7, 1), ParentOrderID = -1 });
            data.Add(new Order() { CustomerID = "15", OrderDate = new DateTime(2015, 4, 1), ParentOrderID = -1 });
            data.Add(new Order() { CustomerID = "16", OrderDate = new DateTime(2015, 4, 1), ParentOrderID = -1 });
            data.Add(new Order() { CustomerID = "17", OrderDate = new DateTime(2015, 4, 1), ParentOrderID = -1 });
            data.Add(new Order() { CustomerID = "12", OrderDate = new DateTime(2015, 8, 1), ParentOrderID = -1 });
            List<Order> expected = new List<Order>();
            expected.Add(data[0]);
            expected.Add(data[2]);
            expected.Add(data[3]);
            expected.Add(data[4]);
            expected.Add(data[8]);
            Mock<DbSet<Order>> mockSet = EntityMockFactory.CreateSet(data.AsQueryable());
            Mock<OrderDb> mockContext = new Mock<OrderDb>();
            mockContext.Setup(c => c.Orders).Returns(mockSet.Object);
            OrderRepository sut = new OrderRepository(mockContext.Object);

            // act
            var actual = sut.GetOrdersByCustomer("12", new DateTime(2015, 1, 1), new DateTime(2016, 1, 1));

            // assert
            Assert.IsTrue(Equality.AreEqual(expected, actual));
        }

        private static List<OrderItem> CreateOrderItemsData()
        {
            List<OrderItem> orderItemsData = new List<OrderItem>();
            orderItemsData.Add(new OrderItem() { ID = 100, OrderID = 5 });
            orderItemsData.Add(new OrderItem() { ID = 101, OrderID = 5 });
            orderItemsData.Add(new OrderItem() { ID = 102, OrderID = 5 });
            orderItemsData.Add(new OrderItem() { ID = 103, OrderID = 5 });
            orderItemsData.Add(new OrderItem() { ID = 104, OrderID = 7 });
            orderItemsData.Add(new OrderItem() { ID = 105, OrderID = 7 });
            orderItemsData.Add(new OrderItem() { ID = 106, OrderID = 8 });
            orderItemsData.Add(new OrderItem() { ID = 107, OrderID = 8 });
            return orderItemsData;
        }

        private static List<Order> CreateOrderData()
        {
            // arrange
            List<Order> orderData = new List<Order>();
            orderData.Add(new Order() { ID = 1 });
            orderData.Add(new Order() { ID = 3 });
            orderData.Add(new Order() { ID = 5 });
            orderData.Add(new Order() { ID = 7, ParentOrderID = 5 });
            orderData.Add(new Order() { ID = 8, ParentOrderID = 5 });
            return orderData;
        }
    }
}