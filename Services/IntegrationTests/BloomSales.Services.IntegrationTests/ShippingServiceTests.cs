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
    public class ShippingServiceTests
    {
        private static ServiceHost shippingSvcHost;
        private ShippingClient shippingClient;

        [ClassCleanup]
        public static void CleanupService()
        {
            shippingSvcHost.Close();

            // drop the Shipping Database
            Database.Delete("ShippingDatabase");
        }

        [ClassInitialize]
        public static void InitializeService(TestContext textContext)
        {
            // drop the Shipping Database if it exists
            if (Database.Exists("ShippingDatabase"))
                Database.Delete("ShippingDatabase");

            // spin up Shipping Service
            shippingSvcHost = new ServiceHost(typeof(ShippingService));
            shippingSvcHost.Open();
        }

        [TestCleanup]
        public void CleanupClient()
        {
            this.shippingClient.Close();
        }

        [TestMethod]
        [TestCategory(TestType.IntegrationTest)]
        [TestCategory("BloomSales.Services.IntegrationTests.ShippingServiceTests")]
        public void GetAllShippers_AtAllTimes_ReturnsTheListOfShippers()
        {
            // arrange

            // act
            var actual = shippingClient.GetAllShippers();

            // assert
            var shippers = new List<Shipper>(actual.OrderBy(s => s.ID));
            Assert.AreEqual(5, shippers.Count);
            Assert.AreEqual("BloomSales", shippers[0].Name);
            Assert.AreEqual("UPS", shippers[1].Name);
            Assert.AreEqual("Canada Post", shippers[2].Name);
            Assert.AreEqual("DHL", shippers[3].Name);
            Assert.AreEqual("Fedex", shippers[4].Name);
        }

        [TestMethod]
        [TestCategory(TestType.IntegrationTest)]
        [TestCategory("BloomSales.Services.IntegrationTests.ShippingServiceTests")]
        public void GetServicesByShippers_GivenAShipperName_ReturnsAllServicesByTheShipper()
        {
            // arrange

            // act
            var actual = shippingClient.GetServicesByShipper("DHL");

            // assert
            var services = new List<DeliveryService>(actual.OrderBy(d => d.ID));
            Assert.AreEqual(2, services.Count);
            Assert.AreEqual("DHL Express", services[0].ServiceName);
            Assert.AreEqual("DHL Global", services[1].ServiceName);
        }

        [TestMethod]
        [TestCategory(TestType.IntegrationTest)]
        [TestCategory("BloomSales.Services.IntegrationTests.ShippingServiceTests")]
        public void GetShippingStatus_GivenAnOrderID_ReturnsTheShippingStatus()
        {
            // arrange
            var request = new ShippingInfo()
            {
                Name = "Not Relevant!",
                City = "Not Relevant!",
                Country = "Not Relevant!",
                Email = "Not Relevant!",
                Phone = "Not Relevant!",
                PostalCode = "Irrelevant",
                Province = "Not Relevant!",
                StreetAddress = "Not Relevant!",
                Status = ShippingStatus.None,
                OrderID = 1212,
                WarehouseID = 7,
                ServiceID = 4
            };

            shippingClient.RequestShipping(request);

            // act
            var actual = shippingClient.GetShippingStatus(1212);

            // assert
            Assert.AreEqual(ShippingStatus.ReceivedOrder, actual);
        }

        [TestInitialize]
        public void InitializeClient()
        {
            this.shippingClient = new ShippingClient();
            shippingClient.Open();
        }

        [TestMethod]
        [TestCategory(TestType.IntegrationTest)]
        [TestCategory("BloomSales.Services.IntegrationTests.ShippingServiceTests")]
        public void RequestShipping_GivenANewShipping_AddsTheRequestToTheDatabase()
        {
            // arrange
            var request = new ShippingInfo()
            {
                Name = "Not Relevant!",
                City = "Not Relevant!",
                Country = "Not Relevant!",
                Email = "Not Relevant!",
                Phone = "Not Relevant!",
                PostalCode = "Irrelevant",
                Province = "Not Relevant!",
                StreetAddress = "Not Relevant!",
                Status = ShippingStatus.None,
                OrderID = 4141,
                WarehouseID = 7,
                ServiceID = 4
            };

            // act
            shippingClient.RequestShipping(request);

            // assert
            var status = shippingClient.GetShippingStatus(4141);
            Assert.AreEqual(ShippingStatus.ReceivedOrder, status);
        }
    }
}