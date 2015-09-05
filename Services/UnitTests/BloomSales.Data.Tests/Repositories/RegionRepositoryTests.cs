using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BloomSales.Data.Entities;
using Moq;
using System.Data.Entity;
using BloomSales.Data.Repositories;
using BloomSales.TestHelpers;

namespace BloomSales.Data.Tests.Repositories
{
    [TestClass]
    public class RegionRepositoryTests
    {
        [TestMethod]
        public void GetAllRegions_OnNonEmptyTable_ReturnsAllRegions()
        {
            // arrange
            List<Region> data = new List<Region>();
            data.Add(new Region { ID = 1, Continent = "North America", Country = "Canada", Name = "Eastern Canada", Provinces = null });
            data.Add(new Region { ID = 2, Continent = "North America", Country = "Canada", Name = "Northern Canada", Provinces = null });
            data.Add(new Region { ID = 3, Continent = "North America", Country = "Canada", Name = "Western Canada", Provinces = null });
            Mock<DbSet<Region>> mockSet = EntityMockFactory.CreateSet(data.AsQueryable());
            Mock<LocationDb> mockContext = new Mock<LocationDb>();
            mockContext.Setup(c => c.Regions).Returns(mockSet.Object);
            RegionRepository sut = new RegionRepository(mockContext.Object);
            
            // act
            IEnumerable<Region> actual = sut.GetAllRegions();

            // assert
            Assert.IsTrue(Equality.AreEqual(data, actual));
        }

        [TestMethod]
        public void GetAllRegionsByCountry_GivenAValidCountryOnNonEmptyRecords_RetunsTheRelatedRegions()
        {
            // arrange
            List<Region> data = new List<Region>();
            data.Add(new Region() { ID = 1, Country = "Canada" });
            data.Add(new Region() { ID = 2, Country = "US" });
            data.Add(new Region() { ID = 3, Country = "Canada" });
            data.Add(new Region() { ID = 4, Country = "Canada" });
            List<Region> expected = new List<Region>();
            expected.Add(data[0]);
            expected.Add(data[2]);
            expected.Add(data[3]);
            Mock<DbSet<Region>> mockSet = EntityMockFactory.CreateSet(data.AsQueryable());
            Mock<LocationDb> mockContext = new Mock<LocationDb>();
            mockContext.Setup(c => c.Regions).Returns(mockSet.Object);
            RegionRepository sut = new RegionRepository(mockContext.Object);

            // act
            IEnumerable<Region> actual = sut.GetAllRegionsByCountry("Canada");

            // assert
            Assert.IsTrue(Equality.AreEqual(expected, actual));
        }

        [TestMethod]
        public void GetRegion_GivenANameOnNonEmptyDatabase_ReturnsTheRegionRecord()
        {
            // arrange
            List<Region> data = new List<Region>();
            Region expected = new Region { ID = 1, Continent = "North America", Country = "Canada", Name = "Eastern Canada", Provinces = null };
            data.Add(expected);
            Mock<DbSet<Region>> mockSet = EntityMockFactory.CreateSet(data.AsQueryable());
            Mock<LocationDb> mockContext = new Mock<LocationDb>();
            mockContext.Setup(c => c.Regions).Returns(mockSet.Object);
            RegionRepository sut = new RegionRepository(mockContext.Object);

            // act
            Region actual = sut.GetRegion("Eastern Canada");

            // assert
            Assert.IsTrue(expected.Equals(actual));
        }

        [TestMethod]
        public void GetRegion_GivenAnIDOnNonEmptyDatabase_ReturnsTheRegionRecord()
        {
            // arrange
            List<Region> data = new List<Region>();
            Region expected = new Region { ID = 1, Continent = "North America", Country = "Canada", Name = "Eastern Canada", Provinces = null };
            data.Add(expected);
            Mock<DbSet<Region>> mockSet = EntityMockFactory.CreateSet(data.AsQueryable());
            mockSet.Setup(s => s.Find(It.Is<int>(id => id == 1))).Returns(expected);
            Mock<LocationDb> mockContext = new Mock<LocationDb>();
            mockContext.Setup(c => c.Regions).Returns(mockSet.Object);
            RegionRepository sut = new RegionRepository(mockContext.Object);

            // act
            Region actual = sut.GetRegion(1);

            // assert
            mockSet.Verify(s => s.Find(It.Is<int>(id => id == 1)), Times.Once());
            Assert.IsTrue(expected.Equals(actual));
        }

        [TestMethod]
        public void AddRegion_OnAnEmptyTable_AddsTheNewRecord()
        {
            // arrange
            List<Region> data = new List<Region>();
            Mock<DbSet<Region>> mockSet = EntityMockFactory.CreateSet(data.AsQueryable());
            Mock<LocationDb> mockContext = new Mock<LocationDb>();
            mockContext.Setup(c => c.Regions).Returns(mockSet.Object);
            RegionRepository sut = new RegionRepository(mockContext.Object);

            // act
            Region region = new Region { ID = 1, Continent = "North America", Country = "Canada", Name = "Eastern Canada", Provinces = null };
            sut.AddRegion(region);

            // assert
            mockSet.Verify(s => s.Add(It.IsAny<Region>()), Times.Once());
            mockContext.Verify(c => c.SaveChanges(), Times.Once());
        }
    }
}
