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
    public class ShipperRepositoryTests
    {
        [TestMethod]
        public void AddShipper_GivenANewShipper_AddsToDatabase()
        {
            // arrange
            Mock<DbSet<Shipper>> mockSet = new Mock<DbSet<Shipper>>();
            Mock<ShippingDb> mockContext = new Mock<ShippingDb>();
            mockContext.Setup(c => c.Shippers).Returns(mockSet.Object);
            ShipperRepository sut = new ShipperRepository(mockContext.Object);
            Shipper shipper = new Shipper() { ID = 200 };

            // act
            sut.AddShipper(shipper);

            // assert
            mockSet.Verify(s => s.Add(It.Is<Shipper>(a => a.ID == 200)), Times.Once());
            mockContext.Verify(c => c.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void GetAllShippers_Always_ReturnsTheWholeShipperRecordsFromDatabase()
        {
            // arrange
            List<Shipper> expected = new List<Shipper>();
            expected.Add(new Shipper() { ID = 100 });
            Mock<DbSet<Shipper>> mockSet = EntityMockFactory.CreateSet(expected.AsQueryable());
            Mock<ShippingDb> mockContext = new Mock<ShippingDb>();
            mockContext.Setup(c => c.Shippers).Returns(mockSet.Object);
            ShipperRepository sut = new ShipperRepository(mockContext.Object);

            // act
            var actual = sut.GetAllShippers();

            // assert
            Assert.IsTrue(Equality.AreEqual(expected, actual));
        }

        [TestMethod]
        public void GetShipper_GivenAValidName_ReturnsTheRecordFromDatabase()
        {
            // arrange
            List<Shipper> data = new List<Shipper>();
            data.Add(new Shipper() { ID = 100, Name = "DHL" });
            Mock<DbSet<Shipper>> mockSet = EntityMockFactory.CreateSet(data.AsQueryable());
            Mock<ShippingDb> mockContext = new Mock<ShippingDb>();
            mockContext.Setup(c => c.Shippers).Returns(mockSet.Object);
            ShipperRepository sut = new ShipperRepository(mockContext.Object);

            // act
            Shipper actual = sut.GetShipper("DHL");

            // assert
            Assert.AreEqual(data[0], actual);
        }
    }
}
