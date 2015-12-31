using BloomSales.Data.Entities;
using BloomSales.Data.Repositories;
using BloomSales.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Data.Entity;

namespace BloomSales.Data.Tests.Repositories
{
    [TestClass]
    public class ShippingInfoRepositoryTests
    {
        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        public void AddShipping_GivenANewShipping_AddsToDatabase()
        {
            // arrange
            Mock<DbSet<ShippingInfo>> mockSet = new Mock<DbSet<ShippingInfo>>();
            Mock<ShippingDb> mockContext = new Mock<ShippingDb>();
            mockContext.Setup(c => c.Shippings).Returns(mockSet.Object);
            ShippingInfo shipping = new ShippingInfo() { OrderID = 1000 };
            ShippingInfoRepository sut = new ShippingInfoRepository(mockContext.Object);

            // act
            sut.AddShipping(shipping);

            // assert
            mockSet.Verify(s => s.Add(It.Is<ShippingInfo>(a => a.OrderID == 1000)), Times.Once());
            mockContext.Verify(c => c.SaveChanges(), Times.Once());
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        public void GetShippingStatus_GivenAValidOrderID_ReturnsTheShippingStatus()
        {
            // arrange
            ShippingInfo shipping = new ShippingInfo() { Status = ShippingStatus.InTransit };
            Mock<DbSet<ShippingInfo>> mockSet = new Mock<DbSet<ShippingInfo>>();
            mockSet.Setup(s => s.Find(It.Is<int>(a => a == 300))).Returns(shipping);
            Mock<ShippingDb> mockContext = new Mock<ShippingDb>();
            mockContext.Setup(c => c.Shippings).Returns(mockSet.Object);
            ShippingInfoRepository sut = new ShippingInfoRepository(mockContext.Object);

            // act
            ShippingStatus actual = sut.GetShippingStatus(300);

            // assert
            mockSet.Verify(s => s.Find(It.Is<int>(a => a == 300)), Times.Once());
            Assert.AreEqual(ShippingStatus.InTransit, actual);
        }
    }
}