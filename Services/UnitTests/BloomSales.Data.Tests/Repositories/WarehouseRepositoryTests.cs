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
    public class WarehouseRepositoryTests
    {
        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        public void GetWarehousesByRegion_OnNonEmptyRecords_ReturnsTheRelatedRecords()
        {
            // arrange
            List<Warehouse> data = new List<Warehouse>();
            data.Add(new Warehouse() { ID = 1, RegionID = 2 });
            data.Add(new Warehouse() { ID = 2, RegionID = 1 });
            data.Add(new Warehouse() { ID = 3, RegionID = 2 });
            data.Add(new Warehouse() { ID = 4, RegionID = 1 });
            data.Add(new Warehouse() { ID = 5, RegionID = 1 });
            List<Warehouse> expected = new List<Warehouse>();
            expected.Add(data[0]);
            expected.Add(data[2]);
            Region region = new Region() { ID = 2, Name = "A Test Region" };
            Mock<IRegionRepository> mockRepo = new Mock<IRegionRepository>();
            mockRepo.Setup(r => r.GetRegion(It.IsIn("A Test Region"))).Returns(region);
            Mock<DbSet<Warehouse>> mockSet = EntityMockFactory.CreateSet(data.AsQueryable());
            Mock<LocationDb> mockContext = new Mock<LocationDb>();
            mockContext.Setup(c => c.Warehouses).Returns(mockSet.Object);
            WarehouseRepository sut = new WarehouseRepository(mockContext.Object, mockRepo.Object);

            // act
            IEnumerable<Warehouse> actual = sut.GetWarehousesByRegion("A Test Region");

            // assert
            mockRepo.Verify(r => r.GetRegion(It.IsIn("A Test Region")), Times.Once());
            Assert.IsTrue(Equality.AreEqual(expected, actual));
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        public void GetWarehousesByCity_OnNonEmptyRecords_ReturnsRelatedRecords()
        {
            // arrange
            List<Warehouse> data = new List<Warehouse>();
            data.Add(new Warehouse() { ID = 1, City = "Montreal" });
            data.Add(new Warehouse() { ID = 2, City = "Ottawa" });
            data.Add(new Warehouse() { ID = 3, City = "Toronto" });
            data.Add(new Warehouse() { ID = 4, City = "Montreal" });
            data.Add(new Warehouse() { ID = 5, City = "Calgary" });
            List<Warehouse> expected = new List<Warehouse>();
            expected.Add(data[0]);
            expected.Add(data[3]);
            Mock<DbSet<Warehouse>> mockSet = EntityMockFactory.CreateSet(data.AsQueryable());
            Mock<LocationDb> mockContext = new Mock<LocationDb>();
            mockContext.Setup(c => c.Warehouses).Returns(mockSet.Object);
            WarehouseRepository sut = new WarehouseRepository(mockContext.Object, null);

            // act
            IEnumerable<Warehouse> actual = sut.GetWarehousesByCity("Montreal");

            // assert
            Assert.IsTrue(Equality.AreEqual(expected, actual));
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        public void GetWarehousesByProvince_OnNonEmptyRecords_ReturnsRelatedRecords()
        {
            // arrange
            List<Warehouse> data = new List<Warehouse>();
            data.Add(new Warehouse() { ID = 1, Province = "Alberta" });
            data.Add(new Warehouse() { ID = 2, Province = "Prince Edward Island" });
            data.Add(new Warehouse() { ID = 3, Province = "British Columbia" });
            data.Add(new Warehouse() { ID = 4, Province = "Quebec" });
            data.Add(new Warehouse() { ID = 5, Province = "Alberta" });
            data.Add(new Warehouse() { ID = 6, Province = "Newfoundland & Labrador" });
            List<Warehouse> expected = new List<Warehouse>();
            expected.Add(data[0]);
            expected.Add(data[4]);
            Mock<DbSet<Warehouse>> mockSet = EntityMockFactory.CreateSet(data.AsQueryable());
            Mock<LocationDb> mockContext = new Mock<LocationDb>();
            mockContext.Setup(c => c.Warehouses).Returns(mockSet.Object);
            WarehouseRepository sut = new WarehouseRepository(mockContext.Object, null);

            // act
            IEnumerable<Warehouse> actual = sut.GetWarehousesByProvince("Alberta");

            // assert
            Assert.IsTrue(Equality.AreEqual(expected, actual));
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        public void GetWarehouse_GivenANameOnNonEmptyRecords_RetunsTheWarehouseRecord()
        {
            // arrange
            List<Warehouse> data = new List<Warehouse>();
            data.Add(new Warehouse() { ID = 1, Name = "Warehouse 1" });
            data.Add(new Warehouse() { ID = 2, Name = "Warehouse 2" });
            data.Add(new Warehouse() { ID = 3, Name = "Warehouse 3" });
            data.Add(new Warehouse() { ID = 4, Name = "Warehouse 4" });
            data.Add(new Warehouse() { ID = 5, Name = "Warehouse 5" });
            Warehouse expected = data[2];
            Mock<DbSet<Warehouse>> mockSet = EntityMockFactory.CreateSet(data.AsQueryable());
            Mock<LocationDb> mockContext = new Mock<LocationDb>();
            mockContext.Setup(c => c.Warehouses).Returns(mockSet.Object);
            WarehouseRepository sut = new WarehouseRepository(mockContext.Object, null);

            // act
            Warehouse actual = sut.GetWarehouse("Warehouse 3");

            // assert
            Assert.IsTrue(expected.Equals(actual));
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        public void GetWarehouse_GivenAnIDOnNonEmptyRecords_RetunsTheWarehouseRecord()
        {
            // arrange
            List<Warehouse> data = new List<Warehouse>();
            data.Add(new Warehouse() { ID = 1, Name = "Warehouse 1" });
            data.Add(new Warehouse() { ID = 2, Name = "Warehouse 2" });
            data.Add(new Warehouse() { ID = 3, Name = "Warehouse 3" });
            data.Add(new Warehouse() { ID = 4, Name = "Warehouse 4" });
            data.Add(new Warehouse() { ID = 5, Name = "Warehouse 5" });
            Warehouse expected = data[2];
            Mock<DbSet<Warehouse>> mockSet = EntityMockFactory.CreateSet(data.AsQueryable());
            mockSet.Setup(s => s.Find(It.IsIn(2))).Returns(data[2]);
            Mock<LocationDb> mockContext = new Mock<LocationDb>();
            mockContext.Setup(c => c.Warehouses).Returns(mockSet.Object);
            WarehouseRepository sut = new WarehouseRepository(mockContext.Object, null);

            // act
            Warehouse actual = sut.GetWarehouse(2);

            // assert
            mockSet.Verify(s => s.Find(It.IsIn(2)), Times.Once());
            Assert.IsTrue(expected.Equals(actual));
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        public void AddWarehouse_GivenANewWarehouse_AddsToRecords()
        {
            // assert
            List<Warehouse> data = new List<Warehouse>();
            Warehouse newRecord = new Warehouse() { ID = 10 };
            Mock<DbSet<Warehouse>> mockSet = EntityMockFactory.CreateSet(data.AsQueryable());
            Mock<LocationDb> mockContext = new Mock<LocationDb>();
            mockContext.Setup(c => c.Warehouses).Returns(mockSet.Object);
            WarehouseRepository sut = new WarehouseRepository(mockContext.Object, null);

            // act
            sut.AddWarehouse(newRecord);

            // assert
            mockSet.Verify(s => s.Add(It.Is<Warehouse>(a => a.ID == 10)), Times.Once());
            mockContext.Verify(c => c.SaveChanges(), Times.Once());
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        public void RemoveWarehouse_GivenAnInstance_RemovesTheRecord()
        {
            // assert
            List<Warehouse> data = new List<Warehouse>();
            data.Add(new Warehouse() { ID = 10 });
            Warehouse existingRecord = new Warehouse() { ID = 10 };
            Mock<DbSet<Warehouse>> mockSet = EntityMockFactory.CreateSet(data.AsQueryable());
            Mock<LocationDb> mockContext = new Mock<LocationDb>();
            mockContext.Setup(c => c.Warehouses).Returns(mockSet.Object);
            WarehouseRepository sut = new WarehouseRepository(mockContext.Object, null);

            // act
            sut.RemoveWarehouse(existingRecord);

            // assert
            mockSet.Verify(s => s.Attach(It.Is<Warehouse>(a => a.ID == 10)), Times.Once());
            mockSet.Verify(s => s.Remove(It.Is<Warehouse>(a => a.ID == 10)), Times.Once());
            mockContext.Verify(c => c.SaveChanges(), Times.Once());
        }
    }
}