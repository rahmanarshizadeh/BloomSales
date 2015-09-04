using BloomSales.Data.Entities;
using BloomSales.Data.Repositories;
using BloomSales.Services.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace BloomSales.Services.Tests
{
    [TestClass]
    public class OrderServiceTests
    {
        [TestMethod]
        public void PlaceOrder_GivenANewOrderShippingAndPayment_PlacesTheOrder()
        {
            // arrange
            Order order = new Order()
            {
                Items = new OrderItem[]
                {
                    new OrderItem() { ProductID = 1, Quantity = 1 },
                    new OrderItem() { ProductID = 2, Quantity = 1 },
                    new OrderItem() { ProductID = 3, Quantity = 1 },
                    new OrderItem() { ProductID = 4, Quantity = 1 },
                }
            };
            ShippingInfo shipping = new ShippingInfo() { City = "Montreal" };
            PaymentInfo payment = new PaymentInfo() { Amount = 87 };
            IEnumerable<Warehouse> warehouses = new Warehouse[]
            {
                new Warehouse() { City = "Montreal", ID = 10, Name = "W11" }
            };
            Mock<IAccountingService> mockAccountingService = new Mock<IAccountingService>();
            mockAccountingService.Setup(s => s.ProcessPayment(It.Is<PaymentInfo>(p => p.Amount == 87)))
                .Returns(true);
            Mock<ILocationService> mockLocationService = new Mock<ILocationService>();
            mockLocationService.Setup(s => s.GetWarehousesByCity("Montreal")).Returns(warehouses);
            Mock<IInventoryService> mockInventoryService = new Mock<IInventoryService>();
            mockInventoryService.Setup(s => s.GetStockByWarehouse("W11", 1)).Returns(new InventoryItem() { ID = 5, UnitsInStock = 2 });
            mockInventoryService.Setup(s => s.GetStockByWarehouse("W11", 2)).Returns(new InventoryItem() { ID = 6, UnitsInStock = 2 });
            mockInventoryService.Setup(s => s.GetStockByWarehouse("W11", 3)).Returns(new InventoryItem() { ID = 7, UnitsInStock = 3 });
            mockInventoryService.Setup(s => s.GetStockByWarehouse("W11", 4)).Returns(new InventoryItem() { ID = 8, UnitsInStock = 4 });
            Mock<IOrderItemRepository> mockOrderItemRepo = new Mock<IOrderItemRepository>();
            Mock<IOrderRepository> mockOrderRepo = new Mock<IOrderRepository>();
            mockOrderRepo.Setup(r => r.AddOrder(It.Is<Order>(o => o.Items.Count() == 4))).Returns(100);
            Mock<IShippingService> mockShippingService = new Mock<IShippingService>();
            OrderService sut = new OrderService(mockLocationService.Object, mockShippingService.Object, mockAccountingService.Object,
                mockInventoryService.Object, mockOrderRepo.Object, mockOrderItemRepo.Object, null);

            // act
            var result = sut.PlaceOrder(order, shipping, payment);

            // assert
            Assert.AreEqual(true, result);
            mockAccountingService.Verify(s => s.ProcessPayment(It.Is<PaymentInfo>(p => p.Amount == 87)), Times.Once());
            mockLocationService.Verify(s => s.GetWarehousesByCity("Montreal"), Times.Once());
            mockInventoryService.Verify(s => s.GetStockByWarehouse("W11", 1), Times.Once());
            mockInventoryService.Verify(s => s.GetStockByWarehouse("W11", 2), Times.Once());
            mockInventoryService.Verify(s => s.GetStockByWarehouse("W11", 3), Times.Once());
            mockInventoryService.Verify(s => s.GetStockByWarehouse("W11", 4), Times.Once());
            mockInventoryService.Verify(s => s.UpdateStock(5, 1), Times.Once());
            mockInventoryService.Verify(s => s.UpdateStock(6, 1), Times.Once());
            mockInventoryService.Verify(s => s.UpdateStock(7, 2), Times.Once());
            mockInventoryService.Verify(s => s.UpdateStock(8, 3), Times.Once());
            mockOrderItemRepo.Verify(r => r.AddItem(It.Is<OrderItem>(oi => oi.ProductID == 1)), Times.Once());
            mockOrderItemRepo.Verify(r => r.AddItem(It.Is<OrderItem>(oi => oi.ProductID == 2)), Times.Once());
            mockOrderItemRepo.Verify(r => r.AddItem(It.Is<OrderItem>(oi => oi.ProductID == 3)), Times.Once());
            mockOrderItemRepo.Verify(r => r.AddItem(It.Is<OrderItem>(oi => oi.ProductID == 4)), Times.Once());
            mockOrderRepo.Verify(r => r.AddOrder(It.Is<Order>(o => o.Items.Count() == 4)), Times.Once());
            mockShippingService.Verify(s => s.RequestShipping(It.Is<ShippingInfo>(si => si.WarehouseID == 10 && si.OrderID == 100)),
                Times.Once());
        }

        [TestMethod]
        public void PlaceOrder_GivenANewOrderWhereOrderItemsAreNotAvailableInASingleWarehouse_PlacesTheOrder()
        {
            // arrange
            Order order = new Order()
            {
                Items = new OrderItem[]
                {
                    new OrderItem() { ProductID = 1, Quantity = 1 },
                    new OrderItem() { ProductID = 2, Quantity = 1 },
                    new OrderItem() { ProductID = 3, Quantity = 1 },
                    new OrderItem() { ProductID = 4, Quantity = 1 },
                }
            };
            ShippingInfo shipping = new ShippingInfo() { City = "Montreal" };
            PaymentInfo payment = new PaymentInfo() { Amount = 87 };
            IEnumerable<Warehouse> warehouses = new Warehouse[]
            {
                new Warehouse() { City = "Montreal", ID = 10, Name = "W11" },
                new Warehouse() { City = "Montreal", ID = 11, Name = "W12" },
                new Warehouse() { City = "Montreal", ID = 12, Name = "W13" },
            };
            IEnumerable<DeliveryService> services = new DeliveryService[]
            {
                new DeliveryService() { ID = 1234 }
            };
            Mock<IAccountingService> mockAccountingService = new Mock<IAccountingService>();
            mockAccountingService.Setup(s => s.ProcessPayment(It.Is<PaymentInfo>(p => p.Amount == 87)))
                .Returns(true);
            Mock<ILocationService> mockLocationService = new Mock<ILocationService>();
            mockLocationService.Setup(s => s.GetWarehousesByCity("Montreal")).Returns(warehouses);
            Mock<IInventoryService> mockInventoryService = new Mock<IInventoryService>();
            mockInventoryService.Setup(s => s.GetStockByWarehouse("W11", 1)).Returns(new InventoryItem() { ID = 5, UnitsInStock = 2 });
            mockInventoryService.Setup(s => s.GetStockByWarehouse("W11", 4)).Returns(new InventoryItem() { ID = 8, UnitsInStock = 0 });
            mockInventoryService.Setup(s => s.GetStockByWarehouse("W12", 1)).Returns(new InventoryItem() { ID = 9, UnitsInStock = 2 });
            mockInventoryService.Setup(s => s.GetStockByWarehouse("W12", 2)).Returns(new InventoryItem() { ID = 10, UnitsInStock = 2 });
            mockInventoryService.Setup(s => s.GetStockByWarehouse("W12", 4)).Returns(new InventoryItem() { ID = 12, UnitsInStock = 0 });
            mockInventoryService.Setup(s => s.GetStockByWarehouse("W13", 1)).Returns(new InventoryItem() { ID = 13, UnitsInStock = 2 });
            mockInventoryService.Setup(s => s.GetStockByWarehouse("W13", 2)).Returns(new InventoryItem() { ID = 14, UnitsInStock = 2 });
            mockInventoryService.Setup(s => s.GetStockByWarehouse("W13", 3)).Returns(new InventoryItem() { ID = 15, UnitsInStock = 3 });
            mockInventoryService.Setup(s => s.GetStockByWarehouse("W13", 4)).Returns(new InventoryItem() { ID = 16, UnitsInStock = 4 });
            Mock<IOrderItemRepository> mockOrderItemRepo = new Mock<IOrderItemRepository>();
            Mock<IOrderRepository> mockOrderRepo = new Mock<IOrderRepository>();
            mockOrderRepo.Setup(r => r.AddOrder(It.Is<Order>(o => o.Items.Count() == 4))).Returns(100);
            Mock<IShippingService> mockShippingService = new Mock<IShippingService>();
            mockShippingService.Setup(s => s.GetServicesByShipper("BloomSales")).Returns(services);
            OrderService sut = new OrderService(mockLocationService.Object, mockShippingService.Object, mockAccountingService.Object,
                mockInventoryService.Object, mockOrderRepo.Object, mockOrderItemRepo.Object, null);

            // act
            var result = sut.PlaceOrder(order, shipping, payment);

            // assert
            Assert.AreEqual(true, result);
            mockAccountingService.Verify(s => s.ProcessPayment(It.Is<PaymentInfo>(p => p.Amount == 87)), Times.Once());
            mockLocationService.Verify(s => s.GetWarehousesByCity("Montreal"), Times.Once());
            mockInventoryService.Verify(s => s.GetStockByWarehouse("W11", 1), Times.Once());
            mockInventoryService.Verify(s => s.GetStockByWarehouse("W11", 2), Times.Once());
            mockInventoryService.Verify(s => s.GetStockByWarehouse("W11", 3), Times.Once());
            mockInventoryService.Verify(s => s.GetStockByWarehouse("W11", 4), Times.Once());
            mockInventoryService.Verify(s => s.GetStockByWarehouse("W12", 1), Times.Never());
            mockInventoryService.Verify(s => s.GetStockByWarehouse("W12", 2), Times.Once());
            mockInventoryService.Verify(s => s.GetStockByWarehouse("W12", 3), Times.Once());
            mockInventoryService.Verify(s => s.GetStockByWarehouse("W12", 4), Times.Once());
            mockInventoryService.Verify(s => s.GetStockByWarehouse("W13", 1), Times.Never());
            mockInventoryService.Verify(s => s.GetStockByWarehouse("W13", 2), Times.Never());
            mockInventoryService.Verify(s => s.GetStockByWarehouse("W13", 3), Times.Once());
            mockInventoryService.Verify(s => s.GetStockByWarehouse("W13", 4), Times.Once());
            mockInventoryService.Verify(s => s.UpdateStock(5, 1), Times.Once());
            mockInventoryService.Verify(s => s.UpdateStock(10, 1), Times.Once());
            mockInventoryService.Verify(s => s.UpdateStock(15, 2), Times.Once());
            mockInventoryService.Verify(s => s.UpdateStock(16, 3), Times.Once());
            mockOrderItemRepo.Verify(r => r.AddItem(It.Is<OrderItem>(oi => oi.ProductID == 1)), Times.Once());
            mockOrderItemRepo.Verify(r => r.AddItem(It.Is<OrderItem>(oi => oi.ProductID == 2)), Times.Once());
            mockOrderItemRepo.Verify(r => r.AddItem(It.Is<OrderItem>(oi => oi.ProductID == 3)), Times.Once());
            mockOrderItemRepo.Verify(r => r.AddItem(It.Is<OrderItem>(oi => oi.ProductID == 4)), Times.Once());
            mockOrderRepo.Verify(r => r.AddOrder(It.Is<Order>(o => o.Items.Count() == 4)), Times.Once());
            mockShippingService.Verify(s => s.RequestShipping(It.Is<ShippingInfo>(si => si.WarehouseID == 10 && si.OrderID == 100)),
                Times.Once());
        }

        [TestMethod]
        public void GetOrder_ResultExistsInCache_ReturnsResultFromCache()
        {
            // arrange
            Order expected = new Order() { ID = 10 };
            Mock<ObjectCache> mockCache = new Mock<ObjectCache>();
            mockCache.Setup(c => c["order#10"]).Returns(expected);
            OrderService sut = new OrderService(null, null, null, null, null, null, mockCache.Object);

            // act
            var actual = sut.GetOrder(10);

            // assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetOrder_ResultNotExistsInCache_FetchesTheResultFromDatabase()
        {
            // arrange
            Order expected = new Order() { ID = 11 };
            Mock<IOrderRepository> mockOrderRepo = new Mock<IOrderRepository>();
            mockOrderRepo.Setup(r => r.GetOrder(11)).Returns(expected);
            Mock<ObjectCache> mockCache = new Mock<ObjectCache>();
            OrderService sut = new OrderService(null, null, null, null, mockOrderRepo.Object, null, mockCache.Object);

            // act
            var actual = sut.GetOrder(11);

            // assert
            Assert.AreEqual(expected, actual);
            mockOrderRepo.Verify(r => r.GetOrder(11), Times.Once());
        }

        [TestMethod]
        public void GetOrder_ResultNotExistsInCache_CachesTheResult()
        {
            // arrange
            Order expected = new Order() { ID = 11 };
            Mock<IOrderRepository> mockOrderRepo = new Mock<IOrderRepository>();
            mockOrderRepo.Setup(r => r.GetOrder(11)).Returns(expected);
            Mock<ObjectCache> mockCache = new Mock<ObjectCache>();
            OrderService sut = new OrderService(null, null, null, null, mockOrderRepo.Object, null, mockCache.Object);

            // act
            sut.GetOrder(11);

            // assert
            mockCache.Verify(c => c.Set("order#11", It.Is<Order>(o => o.Equals(expected)), It.IsAny<CacheItemPolicy>(), null),
                Times.Once());
        }

        [TestMethod]
        public void GetOrderHistoryByCustomer_ResultExistsInCache_ReturnsResultFromCache()
        {
            // arrange
            IEnumerable<Order> expected = new Order[]
            {
                new Order() { ID = 20 },
                new Order() { ID = 30 },
                new Order() { ID = 40 }
            };
            Mock<ObjectCache> mockCache = new Mock<ObjectCache>();
            mockCache.Setup(c => c["c#12Orders(2/1/2015 12:00:00 AM-6/1/2015 12:00:00 AM)"]).Returns(expected);
            OrderService sut = new OrderService(null, null, null, null, null, null, mockCache.Object);

            // act
            var actual = sut.GetOrderHistoryByCustomer(12, new DateTime(2015, 2, 1), new DateTime(2015, 6, 1));

            // assert
            Assert.AreEqual(expected, actual);
            mockCache.Verify(c => c["c#12Orders(2/1/2015 12:00:00 AM-6/1/2015 12:00:00 AM)"], Times.Once());
        }

        [TestMethod]
        public void GetOrderHistoryByCustomer_ResultNotExistsInCache_FetchesTheResultFromDatabase()
        {
            // arrange
            IEnumerable<Order> expected = new Order[]
            {
                new Order() { ID = 30 },
                new Order() { ID = 40 },
                new Order() { ID = 50 },
                new Order() { ID = 60 }
            };
            Mock<ObjectCache> mockCache = new Mock<ObjectCache>();
            Mock<IOrderRepository> mockOrderRepo = new Mock<IOrderRepository>();
            mockOrderRepo.Setup(r => r.GetOrdersByCustomer(21, It.Is<DateTime>(sd => sd.ToShortDateString() == "2/1/2015"),
                It.Is<DateTime>(ed => ed.ToShortDateString() == "6/1/2015"))).Returns(expected);
            OrderService sut = new OrderService(null, null, null, null, mockOrderRepo.Object, null, mockCache.Object);

            // act
            var actual = sut.GetOrderHistoryByCustomer(21, new DateTime(2015, 2, 1), new DateTime(2015, 6, 1));

            // assert
            Assert.AreEqual(expected, actual);
            mockOrderRepo.Verify(r => r.GetOrdersByCustomer(21, It.Is<DateTime>(sd => sd.ToShortDateString() == "2/1/2015"),
                It.Is<DateTime>(ed => ed.ToShortDateString() == "6/1/2015")), Times.Once());
        }

        [TestMethod]
        public void GetOrderHistoryByCustomer_ResultNotExistsInCache_CachesTheResult()
        {
            // arrange
            IEnumerable<Order> expected = new Order[]
            {
                new Order() { ID = 30 },
                new Order() { ID = 40 },
                new Order() { ID = 50 },
                new Order() { ID = 60 }
            };
            Mock<ObjectCache> mockCache = new Mock<ObjectCache>();
            Mock<IOrderRepository> mockOrderRepo = new Mock<IOrderRepository>();
            mockOrderRepo.Setup(r => r.GetOrdersByCustomer(21, It.Is<DateTime>(sd => sd.ToShortDateString() == "2/1/2015"),
                It.Is<DateTime>(ed => ed.ToShortDateString() == "6/1/2015"))).Returns(expected);
            OrderService sut = new OrderService(null, null, null, null, mockOrderRepo.Object, null, mockCache.Object);

            // act
            var actual = sut.GetOrderHistoryByCustomer(21, new DateTime(2015, 2, 1), new DateTime(2015, 6, 1));

            // assert
            mockCache.Verify(
                c => c.Set("c#21Orders(2/1/2015 12:00:00 AM-6/1/2015 12:00:00 AM)", 
                           It.Is<IEnumerable<Order>>(l => l.Equals(expected)),
                           It.IsAny<CacheItemPolicy>(), null),
                Times.Once());
        }
    }
}
