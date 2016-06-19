using BloomSales.Data.Entities;
using BloomSales.Services.Proxies;
using BloomSales.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.ServiceModel;

namespace BloomSales.Services.IntegrationTests
{
    [TestClass]
    public class OrderServiceTests
    {
        private static ServiceHost accountingSvcHost;
        private static ServiceHost shippingSvcHost;
        private static ServiceHost locationSvcHost;
        private static ServiceHost inventorySvcHost;
        private static ServiceHost orderSvcHost;

        private OrderClient orderClient;

        [ClassInitialize]
        public static void InitializeService(TestContext textContext)
        {
            DropDatabases();

            StartServices();
        }

        [TestInitialize]
        public void InitializeClient()
        {
            this.orderClient = new OrderClient();
            orderClient.Open();
        }

        [TestMethod]
        [TestCategory(TestType.IntegrationTest)]
        [TestCategory("BloomSales.Services.IntegrationTests.OrderServiceTests")]
        public void PlaceOrder_GivenANewOrder_ProcessesAndPlacesTheOrder()
        {
            // arrange
            Order order = CreateOrder();
            PaymentInfo payment = CreatePayment();
            ShippingInfo shipping = CreateShipping();

            // act
            var actualResult = orderClient.PlaceOrder(order, shipping, payment);

            // assert
            Assert.IsTrue(actualResult);
            // since this is the only order record in the database, its order ID is 1
            var actualOrder = orderClient.GetOrder(1);
            Assert.IsTrue(actualOrder.HasProcessed);
            var actualItems = new List<OrderItem>(actualOrder.Items);
            Assert.AreEqual(3, actualItems.Count);
        }

        [TestCleanup]
        public void CleanupClient()
        {
            orderClient.Close();
        }

        [ClassCleanup]
        public static void CleanupService()
        {
            orderSvcHost.Close();
            accountingSvcHost.Close();
            shippingSvcHost.Close();
            inventorySvcHost.Close();
            locationSvcHost.Close();

            // drop the Databases
            Database.Delete("AccountingDatabase");
            Database.Delete("ShippingDatabase");
            Database.Delete("LocationDatabase");
            Database.Delete("InventoryDatabase");
            Database.Delete("OrderDatabase");
        }

        private static ShippingInfo CreateShipping()
        {
            return new ShippingInfo()
            {
                Name = "Tamara Mason",
                City = "Montreal",
                Country = "Canada",
                Email = "Not Relevant!",
                Phone = "Not Relevant!",
                PostalCode = "H3X 2R5",
                Province = "QC",
                StreetAddress = "4299 Avenue Van Horne",
                Status = ShippingStatus.None,
                WarehouseID = 7,
                ServiceID = 4
            };
        }

        private static PaymentInfo CreatePayment()
        {
            return new PaymentInfo()
            {
                Amount = 123,
                Currency = "CAD",
                Type = PaymentType.OnlineBanking
            };
        }

        private static Order CreateOrder()
        {
            return new Order()
            {
                CustomerID = 21,
                Items = new List<OrderItem>()
                {
                    new OrderItem()
                    {
                        UnitPrice = 10,
                        Quantity = 2,
                        ProductID = 12,
                    },
                    new OrderItem()
                    {
                        UnitPrice = 10,
                        Quantity = 2,
                        ProductID = 13,
                    },
                    new OrderItem()
                    {
                        UnitPrice = 10,
                        Quantity = 2,
                        ProductID = 17,
                    }
                },
                IsInternalOrder = false,
                OrderDate = DateTime.Now
            };
        }

        private static void StartServices()
        {
            // spin up Shipping Service
            accountingSvcHost = new ServiceHost(typeof(AccountingService));
            accountingSvcHost.Open();
            shippingSvcHost = new ServiceHost(typeof(ShippingService));
            shippingSvcHost.Open();
            locationSvcHost = new ServiceHost(typeof(LocationService));
            locationSvcHost.Open();
            inventorySvcHost = new ServiceHost(typeof(InventoryService));
            inventorySvcHost.Open();
            orderSvcHost = new ServiceHost(typeof(OrderService));
            orderSvcHost.Open();
        }

        private static void DropDatabases()
        {
            // drop the Databases if they exist
            if (Database.Exists("AccountingDatabase"))
                Database.Delete("AccountingDatabase");

            if (Database.Exists("ShippingDatabase"))
                Database.Delete("ShippingDatabase");

            if (Database.Exists("LocationDatabase"))
                Database.Delete("LocationDatabase");

            if (Database.Exists("InventoryDatabase"))
                Database.Delete("InventoryDatabase");

            if (Database.Exists("OrderDatabase"))
                Database.Delete("OrderDatabase");
        }
    }
}