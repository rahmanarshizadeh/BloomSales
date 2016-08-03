using BloomSales.Data.Entities;
using BloomSales.Data.Repositories;
using BloomSales.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace BloomSales.Services.Tests
{
    [TestClass]
    public class LocationServiceTests
    {
        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        [TestCategory("BloomSales.Services.Tests.LocationServiceTests")]
        public void GetAllRegions_ResultExistsInCache_ReturnsTheRelatedRegions()
        {
            // arrange
            IEnumerable<Region> expected = new List<Region>();
            Mock<ObjectCache> mockCache = new Mock<ObjectCache>();
            mockCache.SetupGet(c => c["allRegionsInCanada"]).Returns(expected);
            LocationService sut = new LocationService(null, null, null, mockCache.Object);

            // act
            IEnumerable<Region> actual = sut.GetAllRegions("Canada");

            // assert
            Assert.IsTrue(Equality.AreEqual(expected, actual));
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        [TestCategory("BloomSales.Services.Tests.LocationServiceTests")]
        public void GetAllRegions_ResultNotExistsInCache_FetchesTheResultFromDatabaseAndAddsToCache()
        {
            // arrange
            List<Region> data = new List<Region>();
            data.Add(new Region() { ID = 1, Country = "Canada" });
            data.Add(new Region() { ID = 2, Country = "Canada" });
            data.Add(new Region() { ID = 3, Country = "Germany" });
            data.Add(new Region() { ID = 4, Country = "Canada" });
            List<Region> expected = new List<Region>();
            expected.Add(data[0]);
            expected.Add(data[1]);
            expected.Add(data[3]);
            Mock<IRegionRepository> mockRepo = new Mock<IRegionRepository>();
            mockRepo.Setup(r => r.GetAllRegionsByCountry(It.Is<string>(c => c == "Canada"))).Returns(expected);
            Mock<ObjectCache> mockCache = new Mock<ObjectCache>();
            LocationService sut = new LocationService(mockRepo.Object, null, null, mockCache.Object);

            // act
            IEnumerable<Region> actual = sut.GetAllRegions("Canada");

            // assert
            Assert.IsTrue(Equality.AreEqual(expected, actual));
            mockCache.Verify(
                c => c.Set(
                    It.Is<string>(k => k == "allRegionsInCanada"),
                    It.Is<IEnumerable<Region>>(o => o.Equals(expected)),
                    It.IsAny<CacheItemPolicy>(),
                    null),
                Times.Once());
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        [TestCategory("BloomSales.Services.Tests.LocationServiceTests")]
        public void GetWarehousesByRegion_ResultExistsInCache_ReturnsTheRelateResult()
        {
            // arrange
            List<Warehouse> expected = new List<Warehouse>();
            expected.Add(new Warehouse { ID = 1, Name = "Warehouse 1" });
            Mock<ObjectCache> mockCache = new Mock<ObjectCache>();
            mockCache.Setup(c => c["warehousesInTestRegion"]).Returns(expected);
            LocationService sut = new LocationService(null, null, null, mockCache.Object);

            // act
            IEnumerable<Warehouse> actual = sut.GetWarehousesByRegion("TestRegion");

            // assert
            Assert.IsTrue(Equality.AreEqual(expected, actual));
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        [TestCategory("BloomSales.Services.Tests.LocationServiceTests")]
        public void GetWarehousesByRegion_ResultNotExistsInCache_FetchesTheResultFromDatabaseAndAddsToCache()
        {
            // arrange
            List<Warehouse> data = new List<Warehouse>();
            data.Add(new Warehouse() { ID = 1, RegionID = 1 });
            data.Add(new Warehouse() { ID = 2, RegionID = 3 });
            data.Add(new Warehouse() { ID = 3, RegionID = 2 });
            data.Add(new Warehouse() { ID = 4, RegionID = 2 });
            data.Add(new Warehouse() { ID = 5, RegionID = 1 });
            List<Warehouse> expected = new List<Warehouse>();
            expected.Add(data[0]);
            expected.Add(data[4]);
            Mock<ObjectCache> mockCache = new Mock<ObjectCache>();
            Mock<IWarehouseRepository> mockRepo = new Mock<IWarehouseRepository>();
            mockRepo.Setup(r => r.GetWarehousesByRegion("TestRegion")).Returns(expected);
            LocationService sut = new LocationService(null, mockRepo.Object, null, mockCache.Object);

            // act
            IEnumerable<Warehouse> actual = sut.GetWarehousesByRegion("TestRegion");

            // assert
            Assert.IsTrue(Equality.AreEqual(expected, actual));
            mockCache.Verify(
                c => c.Set(
                    It.Is<string>(k => k == "warehousesInTestRegion"),
                    It.Is<IEnumerable<Warehouse>>(o => o.Equals(expected)),
                    It.IsAny<CacheItemPolicy>(),
                    null),
                Times.Once());
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        [TestCategory("BloomSales.Services.Tests.LocationServiceTests")]
        public void GetNearestWarehousesTo_ResultExistsInCache_ReturnsTheResult()
        {
            // arrange
            Warehouse w = new Warehouse() { Name = "Warehouse1" };
            Mock<ObjectCache> mockCache = new Mock<ObjectCache>();
            List<Warehouse> expected = new List<Warehouse>();
            expected.Add(new Warehouse() { Name = "Warehouse2" });
            mockCache.Setup(c => c["nearestToWarehouse1"]).Returns(expected);
            LocationService sut = new LocationService(null, null, null, mockCache.Object);

            // act
            IEnumerable<Warehouse> actual = sut.GetNearestWarehousesTo(w);

            // assert
            Assert.IsTrue(Equality.AreEqual(expected, actual));
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        [TestCategory("BloomSales.Services.Tests.LocationServiceTests")]
        public void GetWarehousesByCity_ResultExistsInCache_ReturnsTheRelatedResult()
        {
            // arrange
            Mock<ObjectCache> mockCache = new Mock<ObjectCache>();
            List<Warehouse> expected = new List<Warehouse>();
            expected.Add(new Warehouse() { Name = "Warehouse2" });
            mockCache.Setup(c => c["warehousesInToronto"]).Returns(expected);
            LocationService sut = new LocationService(null, null, null, mockCache.Object);

            // act
            IEnumerable<Warehouse> actual = sut.GetWarehousesByCity("Toronto");

            // assert
            Assert.IsTrue(Equality.AreEqual(expected, actual));
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        [TestCategory("BloomSales.Services.Tests.LocationServiceTests")]
        public void GetWarehouseByCity_ResultNotExistsInCache_FetchesTheResultFromDatabaseAndAddsToCache()
        {
            // arrange
            List<Warehouse> data = new List<Warehouse>();
            data.Add(new Warehouse() { ID = 1, City = "Montreal" });
            data.Add(new Warehouse() { ID = 2, City = "Toronto" });
            data.Add(new Warehouse() { ID = 3, City = "Vancouver" });
            data.Add(new Warehouse() { ID = 4, City = "Toronto" });
            data.Add(new Warehouse() { ID = 5, City = "Montreal" });
            List<Warehouse> expected = new List<Warehouse>();
            expected.Add(data[1]);
            expected.Add(data[3]);
            Mock<ObjectCache> mockCache = new Mock<ObjectCache>();
            Mock<IWarehouseRepository> mockRepo = new Mock<IWarehouseRepository>();
            mockRepo.Setup(r => r.GetWarehousesByCity("Toronto")).Returns(expected);
            LocationService sut = new LocationService(null, mockRepo.Object, null, mockCache.Object);

            // act
            IEnumerable<Warehouse> actual = sut.GetWarehousesByCity("Toronto");

            // assert
            Assert.IsTrue(Equality.AreEqual(expected, actual));
            mockCache.Verify(
                c => c.Set(
                    It.Is<string>(k => k == "warehousesInToronto"),
                    It.Is<IEnumerable<Warehouse>>(o => o.Equals(expected)),
                    It.IsAny<CacheItemPolicy>(),
                    null),
                Times.Once());
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        [TestCategory("BloomSales.Services.Tests.LocationServiceTests")]
        public void GetWarehouseByName_ResultExistsInCache_ReturnsTheResult()
        {
            // arrange
            Mock<ObjectCache> mockCache = new Mock<ObjectCache>();
            Warehouse expected = new Warehouse() { Name = "Montreal#9" };
            mockCache.Setup(c => c["Montreal#9Warehouse"]).Returns(expected);
            LocationService sut = new LocationService(null, null, null, mockCache.Object);

            // act
            Warehouse actual = sut.GetWarehouseByName("Montreal#9");

            // assert
            Assert.IsTrue(expected.Equals(actual));
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        [TestCategory("BloomSales.Services.Tests.LocationServiceTests")]
        public void GetWarehouseByName_ResultNotExistsInCache_FetchesTheResultFromDatabaseAndAddsToCache()
        {
            // arrange
            Warehouse expected = new Warehouse() { Name = "Montreal#9" };
            Mock<ObjectCache> mockCache = new Mock<ObjectCache>();
            Mock<IWarehouseRepository> mockRepo = new Mock<IWarehouseRepository>();
            mockRepo.Setup(r => r.GetWarehouse("Montreal#9")).Returns(expected);
            LocationService sut = new LocationService(null, mockRepo.Object, null, mockCache.Object);

            // act
            Warehouse actual = sut.GetWarehouseByName("Montreal#9");

            // assert
            Assert.IsTrue(expected.Equals(actual));
            mockCache.Verify(
                c => c.Set(
                    It.Is<string>(k => k == "Montreal#9Warehouse"),
                    It.Is<Warehouse>(o => o.Equals(expected)),
                    It.IsAny<CacheItemPolicy>(),
                    null),
                Times.Once());
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        [TestCategory("BloomSales.Services.Tests.LocationServiceTests")]
        public void GetWarehouseByID_ResultExistsInCache_ReturnsTheResult()
        {
            // arrange
            Mock<ObjectCache> mockCache = new Mock<ObjectCache>();
            Warehouse expected = new Warehouse() { ID = 10 };
            mockCache.Setup(c => c["Warehouse10"]).Returns(expected);
            LocationService sut = new LocationService(null, null, null, mockCache.Object);

            // act
            Warehouse actual = sut.GetWarehouseByID(10);

            // assert
            Assert.IsTrue(expected.Equals(actual));
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        [TestCategory("BloomSales.Services.Tests.LocationServiceTests")]
        public void GetWarehouseByID_ResultNotExistsInCache_FetchesTheResultFromDatabaseAndAddsToCache()
        {
            // arrange
            Warehouse expected = new Warehouse() { ID = 21 };
            Mock<ObjectCache> mockCache = new Mock<ObjectCache>();
            Mock<IWarehouseRepository> mockRepo = new Mock<IWarehouseRepository>();
            mockRepo.Setup(r => r.GetWarehouse(21)).Returns(expected);
            LocationService sut = new LocationService(null, mockRepo.Object, null, mockCache.Object);

            // act
            Warehouse actual = sut.GetWarehouseByID(21);

            // assert
            Assert.IsTrue(expected.Equals(actual));
            mockCache.Verify(
                c => c.Set(
                    It.Is<string>(k => k == "Warehouse21"),
                    It.Is<Warehouse>(o => o.Equals(expected)),
                    It.IsAny<CacheItemPolicy>(),
                    null),
                Times.Once());
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        [TestCategory("BloomSales.Services.Tests.LocationServiceTests")]
        public void AddRegion_GivenANewRegion_AddsToDatabase()
        {
            // arrange
            Region region = new Region() { ID = 101 };
            Mock<IRegionRepository> mockRepo = new Mock<IRegionRepository>();
            LocationService sut = new LocationService(mockRepo.Object, null, null, null);

            // act
            sut.AddRegion(region);

            // assert
            mockRepo.Verify(r => r.AddRegion(It.Is<Region>(a => a.Equals(region))), Times.Once());
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        [TestCategory("BloomSales.Services.Tests.LocationServiceTests")]
        public void AddWarehouse_GivenANewWarehouse_AddsToDatabase()
        {
            // arrange
            Warehouse warehouse = new Warehouse() { ID = 212 };
            Mock<IWarehouseRepository> mockRepo = new Mock<IWarehouseRepository>();
            LocationService sut = new LocationService(null, mockRepo.Object, null, null);

            // act
            sut.AddWarehouse(warehouse);

            // assert
            mockRepo.Verify(r => r.AddWarehouse(It.Is<Warehouse>(a => a.Equals(warehouse))), Times.Once());
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        [TestCategory("BloomSales.Services.Tests.LocationServiceTests")]
        public void UpdateWarehouse_GivenAWarehouse_UpdateTheRecordInDatabase()
        {
            // arrange
            Warehouse warehouse = new Warehouse() { ID = 213 };
            Mock<IWarehouseRepository> mockRepo = new Mock<IWarehouseRepository>();
            LocationService sut = new LocationService(null, mockRepo.Object, null, null);

            // act
            sut.UpdateWarehouse(warehouse);

            // assert
            mockRepo.Verify(r => r.UpdateWarehouse(It.Is<Warehouse>(a => a.Equals(warehouse))), Times.Once());
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        [TestCategory("BloomSales.Services.Tests.LocationServiceTests")]
        public void RemoveWarehouse_GivenAWarehouse_DeletesTheRecordFromDatabase()
        {
            // arrange
            Warehouse warehouse = new Warehouse() { ID = 214 };
            Mock<IWarehouseRepository> mockRepo = new Mock<IWarehouseRepository>();
            LocationService sut = new LocationService(null, mockRepo.Object, null, null);

            // act
            sut.RemoveWarehouse(warehouse);

            // assert
            mockRepo.Verify(r => r.RemoveWarehouse(It.Is<Warehouse>(a => a.Equals(warehouse))), Times.Once());
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        [TestCategory("BloomSales.Services.Tests.LocationServiceTests")]
        public void GetAllProvinces_GivenAValidCountryName_ReturnsListOfItsProvinces()
        {
            // arrange
            var regionData = new List<Region>()
            {
                new Region() { ID = 1 },
                new Region() { ID = 2 }
            };
            var region1Provinces = new List<Province>()
            {
                new Province() { Name = "Province1", RegionID = 1 },
                new Province() { Name = "Province2", RegionID = 1 }
            };
            var region2Provinces = new List<Province>()
            {
                new Province() { Name = "Province3", RegionID = 2 },
                new Province() { Name = "Province4", RegionID = 2 }
            };
            IEnumerable<Province> expected = new Province[]
            {
                region1Provinces[0], region1Provinces[1], region2Provinces[0], region2Provinces[1]
            };
            Mock<IRegionRepository> regionMock = new Mock<IRegionRepository>();
            regionMock.Setup(r => r.GetAllRegionsByCountry("Country1")).Returns(regionData);
            Mock<IProvinceRepository> provinceMock = new Mock<IProvinceRepository>();
            provinceMock.Setup(p => p.GetProvincesForRegion(1)).Returns(region1Provinces);
            provinceMock.Setup(p => p.GetProvincesForRegion(2)).Returns(region2Provinces);
            Mock<ObjectCache> cacheMock = new Mock<ObjectCache>();
            LocationService sut = new LocationService(regionMock.Object, null, provinceMock.Object, cacheMock.Object);

            // act
            var actual = sut.GetAllProvinces("Country1");

            // assert
            Assert.IsTrue(Equality.AreEqual(expected, actual));
        }
    }
}