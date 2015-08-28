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
    public class DeliveryServiceRepositoryTests
    {
        [TestMethod]
        public void AddService_GivenANewService_AddsToDatabase()
        {
            // arrange
            DeliveryService service = new DeliveryService() { ID = 100 };
            Mock<DbSet<DeliveryService>> mockSet = new Mock<DbSet<DeliveryService>>();
            Mock<ShippingDb> mockContext = new Mock<ShippingDb>();
            mockContext.Setup(c => c.Services).Returns(mockSet.Object);
            DeliveryServiceRepository sut = new DeliveryServiceRepository(mockContext.Object);

            // act
            sut.AddService(service);

            // assert
            mockSet.Verify(s => s.Add(It.Is<DeliveryService>(a => a.ID == 100)), Times.Once());
            mockContext.Verify(c => c.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void GetService_GivenAValidID_ReturnsServiceRecordFromDatabase()
        {
            // arrange
            List<DeliveryService> data = new List<DeliveryService>();
            data.Add(new DeliveryService() { ID = 1, ServiceName = "Service 1" });
            data.Add(new DeliveryService() { ID = 2, ServiceName = "Service 2" });
            data.Add(new DeliveryService() { ID = 3, ServiceName = "Service 3" });
            data.Add(new DeliveryService() { ID = 4, ServiceName = "Service 4" });
            DeliveryService expected = data[2];
            Mock<DbSet<DeliveryService>> mockSet = EntityMockFactory.CreateSet(data.AsQueryable());
            mockSet.Setup(s => s.Find(It.Is<int>(a => a == 3))).Returns(expected);
            Mock<ShippingDb> mockContext = new Mock<ShippingDb>();
            mockContext.Setup(c => c.Services).Returns(mockSet.Object);
            DeliveryServiceRepository sut = new DeliveryServiceRepository(mockContext.Object);

            // act
            DeliveryService actual = sut.GetService(3);

            // assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetServicesByShipper_GivenAValidShipperID_ReturnsServicesByShipperFromDatabase()
        {
            // arrange
            List<DeliveryService> data = new List<DeliveryService>();
            data.Add(new DeliveryService() { ID = 1, ShipperID = 1 });
            data.Add(new DeliveryService() { ID = 2, ShipperID = 2 });
            data.Add(new DeliveryService() { ID = 3, ShipperID = 1 });
            data.Add(new DeliveryService() { ID = 4, ShipperID = 4 });
            data.Add(new DeliveryService() { ID = 5, ShipperID = 1 });
            List<DeliveryService> expected = new List<DeliveryService>();
            expected.Add(data[0]);
            expected.Add(data[2]);
            expected.Add(data[4]);
            Mock<DbSet<DeliveryService>> mockSet = EntityMockFactory.CreateSet(data.AsQueryable());
            Mock<ShippingDb> mockContext = new Mock<ShippingDb>();
            mockContext.Setup(c => c.Services).Returns(mockSet.Object);
            DeliveryServiceRepository sut = new DeliveryServiceRepository(mockContext.Object);

            // act
            IEnumerable<DeliveryService> actual = sut.GetServicesByShipper(1);

            // assert
            Assert.IsTrue(Equality.AreEqual(expected, actual));
        }
    }
}
