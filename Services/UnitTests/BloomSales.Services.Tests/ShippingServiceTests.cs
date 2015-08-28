using BloomSales.Data.Entities;
using BloomSales.Data.Repositories;
using BloomSales.TestHelpers;
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
    public class ShippingServiceTests
    {
        [TestMethod]
        public void GetAllShippers_ResultExistsInCache_ReturnsTheListOfShippers()
        {
            // arrange
            List<Shipper> expected = new List<Shipper>();
            expected.Add(new Shipper() { ID = 1 });
            expected.Add(new Shipper() { ID = 2 });
            expected.Add(new Shipper() { ID = 3 });
            expected.Add(new Shipper() { ID = 4 });
            Mock<ObjectCache> mockCache = new Mock<ObjectCache>();
            mockCache.Setup(c => c["allShippers"]).Returns(expected);
            ShippingService sut = new ShippingService(null, null, null, mockCache.Object);

            // act
            var actual = sut.GetAllShippers();

            // assert
            Assert.IsTrue(Equality.AreEqual(expected, actual));
            mockCache.Verify(c => c["allShippers"], Times.Once());
        }

        [TestMethod]
        public void GetAllShippers_ResultNotExistsInCache_FetchesTheResultFromDatabase()
        {
            // arrange
            List<Shipper> expected = new List<Shipper>();
            expected.Add(new Shipper() { ID = 1 });
            expected.Add(new Shipper() { ID = 2 });
            expected.Add(new Shipper() { ID = 3 });
            expected.Add(new Shipper() { ID = 4 });
            Mock<IShipperRepository> mockShipperRepo = new Mock<IShipperRepository>();
            mockShipperRepo.Setup(r => r.GetAllShippers()).Returns(expected);
            Mock<ObjectCache> mockCache = new Mock<ObjectCache>();
            ShippingService sut = new ShippingService(mockShipperRepo.Object, null, null, mockCache.Object);

            // act
            var actual = sut.GetAllShippers();

            // assert
            Assert.IsTrue(Equality.AreEqual(expected, actual));
            mockShipperRepo.Verify(r => r.GetAllShippers(), Times.Once());
            mockCache.Verify(
                c => c.Set(
                    It.Is<string>(s => s.Equals("allShippers")), 
                    It.Is<IEnumerable<Shipper>>(l => l.Count() == 4),
                    It.IsAny<CacheItemPolicy>(),
                    null),
                Times.Once());
        }

        [TestMethod]
        public void GetServicesByShipper_ResultExistsInCache_ReturnsTheResult()
        {
            List<DeliveryService> expected = new List<DeliveryService>();
            expected.Add(new DeliveryService() { ID = 1, ServiceName = "Regular" });
            expected.Add(new DeliveryService() { ID = 2, ServiceName = "Express" });
            Mock<ObjectCache> mockCache = new Mock<ObjectCache>();
            mockCache.Setup(c => c["servicesByUPS"]).Returns(expected);
            ShippingService sut = new ShippingService(null, null, null, mockCache.Object);

            // act
            var actual = sut.GetServicesByShipper("UPS");

            // assert
            Assert.IsTrue(Equality.AreEqual(expected, actual));
            mockCache.Verify(c => c["servicesByUPS"], Times.Once());
        }

        [TestMethod]
        public void GetServiceByShipper_ResultNotExistsInCahce_FetchesTheResultFromDatabase()
        {
            // arrange
            Shipper ups = new Shipper() { ID = 10, Name = "UPS" };
            Mock<IShipperRepository> mockShipperRepo = new Mock<IShipperRepository>();
            mockShipperRepo.Setup(r => r.GetShipper(It.Is<string>(s => s.Equals("UPS")))).Returns(ups);
            List<DeliveryService> expected = new List<DeliveryService>();
            expected.Add(new DeliveryService() { ID = 1, ServiceName = "Regular", ShipperID = 10 });
            expected.Add(new DeliveryService() { ID = 2, ServiceName = "Express", ShipperID = 10 });
            Mock<IDeliveryServiceRepository> mockServiceRepo = new Mock<IDeliveryServiceRepository>();
            mockServiceRepo.Setup(r => r.GetServicesByShipper(It.Is<int>(i => i == 10))).Returns(expected);
            Mock<ObjectCache> mockCache = new Mock<ObjectCache>();
            ShippingService sut = new ShippingService(mockShipperRepo.Object, null, mockServiceRepo.Object, mockCache.Object);

            // act
            var actual = sut.GetServicesByShipper("UPS");

            // assert
            Assert.IsTrue(Equality.AreEqual(expected, actual));
            mockShipperRepo.Verify(r => r.GetShipper(It.Is<string>(s => s.Equals("UPS"))), Times.Once());
            mockServiceRepo.Verify(r => r.GetServicesByShipper(It.Is<int>(i => i == 10)), Times.Once());
            mockCache.Verify(
                c => c.Set(
                    It.Is<string>(s => s.Equals("servicesByUPS")),
                    It.Is<IEnumerable<DeliveryService>>(l => l.Count() == 2),
                    It.IsAny<CacheItemPolicy>(),
                    null),
                Times.Once());
        }

        [TestMethod]
        public void RequestShipping_GivenANewShipping_ReceivedsAndAddsToDatabase()
        {
            // arrange
            Mock<IShippingInfoRepository> mockShippingRepo = new Mock<IShippingInfoRepository>();
            ShippingInfo shipping = new ShippingInfo() { Status = ShippingStatus.None };
            ShippingService sut = new ShippingService(null, mockShippingRepo.Object, null, null);

            // act
            sut.RequestShipping(shipping);
            
            // assert
            mockShippingRepo.Verify(
                r => r.AddShipping(It.Is<ShippingInfo>(s => s.Status == ShippingStatus.ReceivedOrder)),
                Times.Once());
        }

        [TestMethod]
        public void GetShippingStatus_ResultExistsInCache_ReturnsTheResult()
        {
            // arrange
            Mock<ObjectCache> mockCache = new Mock<ObjectCache>();
            mockCache.Setup(c => c["statusForOrder#12"]).Returns(ShippingStatus.OutForDelivery);
            ShippingService sut = new ShippingService(null, null, null, mockCache.Object);

            // act
            ShippingStatus actual = sut.GetShippingStatus(12);

            // assert
            Assert.AreEqual(ShippingStatus.OutForDelivery, actual);
            mockCache.Verify(c => c["statusForOrder#12"], Times.Once());
        }

        [TestMethod]
        public void GetShippingStatus_ResultNotExistsInCache_FetchesTheResultFromDatabase()
        {
            // arrange
            Mock<ObjectCache> mockCache = new Mock<ObjectCache>();
            Mock<IShippingInfoRepository> mockShippingRepo = new Mock<IShippingInfoRepository>();
            mockShippingRepo.Setup(r => r.GetShippingStatus(It.Is<int>(i => i == 210))).Returns(ShippingStatus.InTransit);
            ShippingService sut = new ShippingService(null, mockShippingRepo.Object, null, mockCache.Object);

            // act
            ShippingStatus actual = sut.GetShippingStatus(210);

            // assert
            Assert.AreEqual(ShippingStatus.InTransit, actual);
            mockCache.Verify(c => c["statusForOrder#210"], Times.Once());
        }

        [TestMethod]
        public void AddShipper_GivenANewShipper_AddsToDatabase()
        {
            // arrange
            Shipper shipper = new Shipper() { ID = 3, Name = "DHL" };
            Mock<IShipperRepository> mockShipperRepo = new Mock<IShipperRepository>();
            ShippingService sut = new ShippingService(mockShipperRepo.Object, null, null, null);

            // act
            sut.AddShipper(shipper);

            // assert
            mockShipperRepo.Verify(
                r => r.AddShipper(It.Is<Shipper>(s => s.ID == 3 && s.Name.Equals("DHL"))),
                Times.Once());
        }

        [TestMethod]
        public void AddDeliveryService_GivenANewService_AddsToDatabase()
        {
            // arrange
            DeliveryService service = new DeliveryService() { ID = 2, ShipperID = 3 };
            Mock<IDeliveryServiceRepository> mockServiceRepo = new Mock<IDeliveryServiceRepository>();
            ShippingService sut = new ShippingService(null, null, mockServiceRepo.Object, null);

            // act
            sut.AddDeliveryService(service);

            // assert
            mockServiceRepo.Verify(
                r => r.AddService(It.Is<DeliveryService>(s => s.ID == 2 && s.ShipperID == 3)),
                Times.Once());
        }
    }
}
