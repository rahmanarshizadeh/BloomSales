using BloomSales.Data.Entities;
using BloomSales.Services.Contracts;
using BloomSales.Services.Proxies;
using BloomSales.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Runtime.Caching;
using System.ServiceModel;

namespace BloomSales.Services.IntegrationTests
{
    [TestClass]
    public class LocationServiceTests
    {
        private static ServiceHost host;
        private LocationClient locationClient;

        [ClassInitialize]
        public static void IntinializeService(TestContext context)
        {
            // drop the LocationDatabase if it exists
            if (Database.Exists("LocationDatabase"))
                Database.Delete("LocationDatabase");

            // instantiate and run the service
            host = new ServiceHost(typeof(LocationService));
            host.Open();
        }

        [TestInitialize]
        public void InitializeClient()
        {
            locationClient = new LocationClient();
        }

        [TestMethod]
        [TestCategory(TestType.IntegrationTest)]
        public void GetAllRegions_GivenCountryCanada_ReturnsListOfRegions()
        {
            // arrange

            // act
            var actual = locationClient.GetAllRegions("Canada");

            // assert
            List<Region> regions = new List<Region>(actual);
            regions = regions.OrderBy(r => r.Name).ToList();
            Assert.AreEqual(regions.Count, 3);
            Assert.AreEqual(regions[0].Name, "Eastern Canada");
            Assert.AreEqual(regions[1].Name, "Northern Canada");
            Assert.AreEqual(regions[2].Name, "Western Canada");
        }

        [TestMethod]
        [TestCategory(TestType.IntegrationTest)]
        public void GetWarehousesByRegion_GivenWesternCanada_ReturnsListOfWarehouses()
        {
            // arrange

            // act
            var actual = locationClient.GetWarehousesByRegion("Western Canada");

            // assert
            List<Warehouse> warehouses = new List<Warehouse>(actual);
            warehouses = warehouses.OrderBy(w => w.Name).ToList();
            Assert.AreEqual(warehouses.Count, 2);
            Assert.AreEqual(warehouses[0].Name, "BloomSales W#5");
            Assert.AreEqual(warehouses[1].Name, "BloomSales W#6");
        }

        [TestMethod]
        [TestCategory(TestType.IntegrationTest)]
        public void GetNearestWarehouseTo_GivenATorontoWarehouse_ReturnsTheOtherTorontoWarehouse()
        {
            // arrange
            Warehouse warehouse = new Warehouse()
            {
                Name = "BloomSales W#3",
                City = "Toronto",
                Province = "ON"
            };

            // act
            var actual = locationClient.GetNearestWarehousesTo(warehouse);

            // assert
            List<Warehouse> warehouses = new List<Warehouse>(actual);
            Assert.AreEqual(warehouses.Count, 1);
            Assert.AreEqual(warehouses[0].Name, "BloomSales W#4");
            Assert.AreEqual(warehouses[0].City, "Toronto");
        }

        [TestMethod]
        [TestCategory(TestType.IntegrationTest)]
        public void GetWarehousesByCity_GivenVancouver_ReturnsListOfWarehouses()
        {
            // arrange

            // act
            var actual = locationClient.GetWarehousesByCity("Vancouver");

            // assert
            List<Warehouse> warehouses = new List<Warehouse>(actual);
            Assert.AreEqual(warehouses.Count, 1);
            Assert.AreEqual(warehouses[0].Name, "BloomSales W#6");
        }

        [TestMethod]
        [TestCategory(TestType.IntegrationTest)]
        public void GetWarehouseByName_GivenAValidWarehouseName_ReturnsTheWarehouseObject()
        {
            // arrange

            // act
            var actual = locationClient.GetWarehouseByName("BloomSales W#1");

            // assert
            Assert.AreEqual(actual.Name, "BloomSales W#1");
            Assert.AreEqual(actual.Phone, "1-514-923-9876");
            Assert.AreEqual(actual.PostalCode, "H3G 2H6");
        }

        [TestMethod]
        [TestCategory(TestType.IntegrationTest)]
        public void GetWarehouseByID_GivenAValidID_ReturnsTheWarehouseObject()
        {
            // arrange

            // act
            var actual = locationClient.GetWarehouseByID(7);

            // assert
            Assert.AreEqual(actual.City, "Halifax");
            Assert.AreEqual(actual.Name, "BloomSales W#7");
            Assert.AreEqual(actual.PostalCode, "B3L 5P7");
        }

        [TestMethod]
        [TestCategory(TestType.IntegrationTest)]
        public void AddRegion_GivenANewRegion_AddsToTheDatabase()
        {
            // Remove the record related to "All Canadian Regions" from
            // the cache object before test arrangement, because it will
            // fail the test when LocationService.GetAllRegions() is called.
            MemoryCache cache = MemoryCache.Default;
            cache.Remove("allRegionsInCanada");

            // arrange
            Region region = new Region() { Name = "New Region", Country = "Canada", Continent = "North America" };

            // act
            locationClient.AddRegion(region);

            // assert
            var regionsList = locationClient.GetAllRegions("Canada");
            List<Region> regions = new List<Region>(regionsList);
            regions = regions.OrderBy(r => r.Name).ToList();
            Assert.AreEqual(regions.Count, 4);
            Assert.AreEqual(regions[1].Name, "New Region");
        }

        [TestCleanup]
        public void ClientCleanup()
        {
            locationClient.Close();
        }

        [ClassCleanup]
        public static void ServiceCleanup()
        {
            host.Close();

            // drop the testing Location database
            Database.Delete("LocationDatabase");
        }
    }
}