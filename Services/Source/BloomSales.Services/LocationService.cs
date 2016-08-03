using BloomSales.Data.Entities;
using BloomSales.Data.Repositories;
using BloomSales.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.ServiceModel;

namespace BloomSales.Services
{
    [ServiceBehavior(UseSynchronizationContext = false,
                     ConcurrencyMode = ConcurrencyMode.Multiple,
                     InstanceContextMode = InstanceContextMode.PerCall)]
    public class LocationService : ILocationService, IDisposable
    {
        private IRegionRepository regionRepo;
        private IWarehouseRepository warehouseRepo;
        private IProvinceRepository provinceRepo;
        private ObjectCache cache;
        private CacheItemPolicy defaultPolicy;

        public LocationService()
        {
            this.regionRepo = new RegionRepository();
            this.warehouseRepo = new WarehouseRepository();
            this.provinceRepo = new ProvinceRepository();
            this.cache = MemoryCache.Default;

            this.defaultPolicy = new CacheItemPolicy();
            // set the expiration time to 1 minute
            this.defaultPolicy.SlidingExpiration = new TimeSpan(0, 1, 0);
        }

        public LocationService(IRegionRepository regionRepo,
                               IWarehouseRepository warehouseRepo,
                               IProvinceRepository provinceRepo,
                               ObjectCache cache)
        {
            this.regionRepo = regionRepo;
            this.warehouseRepo = warehouseRepo;
            this.provinceRepo = provinceRepo;
            this.cache = cache;
        }

        public IEnumerable<Province> GetAllProvinces(string country)
        {
            string cacheKey = "provincesOf" + country;

            var result = cache[cacheKey] as List<Province>;

            if (result == null)
            {
                // get all regions
                var regions = regionRepo.GetAllRegionsByCountry(country);
                List<IEnumerable<Province>> provinces = new List<IEnumerable<Province>>();
                IEnumerable<Province> regionalProvinces;
                result = new List<Province>();

                // get provinces for each region
                foreach (var region in regions)
                {
                    regionalProvinces = provinceRepo.GetProvincesForRegion(region.ID);

                    provinces.Add(regionalProvinces);
                }

                // add all provinces into a single list
                for (int i = 0; i < provinces.Count; i++)
                    foreach (var p in provinces[i])
                        result.Add(p);

                // cache the result
                cache.Set(cacheKey, result, CachingPolicies.OneDayPolicy);
            }

            return result;
        }

        public IEnumerable<Region> GetAllRegions(string country)
        {
            string cacheKey = "allRegionsIn" + country;

            var regions = cache[cacheKey] as IEnumerable<Region>;

            if (regions == null)
            {
                regions = this.regionRepo.GetAllRegionsByCountry(country);

                CacheItemPolicy policy = new CacheItemPolicy();
                // doesn't change very often, so set the expiration time to 1 day
                policy.SlidingExpiration = new TimeSpan(1, 0, 0, 0);
                cache.Set(cacheKey, regions, policy);
            }

            return regions;
        }

        public IEnumerable<Warehouse> GetWarehousesByRegion(string region)
        {
            string cacheKey = "warehousesIn" + region;

            var warehouses = cache[cacheKey] as IEnumerable<Warehouse>;

            if (warehouses == null)
            {
                warehouses = this.warehouseRepo.GetWarehousesByRegion(region);

                CacheItemPolicy policy = new CacheItemPolicy();
                // doesn't change very often, set it to 1 day
                policy.SlidingExpiration = new TimeSpan(1, 0, 0, 0);
                this.cache.Set(cacheKey, warehouses, policy);
            }

            return warehouses;
        }

        public IEnumerable<Warehouse> GetNearestWarehousesTo(Warehouse warehouse)
        {
            string cacheKey = "nearestTo" + warehouse.Name;

            var warehouses = cache[cacheKey] as IEnumerable<Warehouse>;

            if (warehouses == null)
            {
                warehouses = FindNearestWarehousesTo(warehouse);

                // set the result in cache
                CacheItemPolicy policy = new CacheItemPolicy();
                // doesn't change very often, so set expiration time to 1 day
                policy.SlidingExpiration = new TimeSpan(1, 0, 0, 0);
                this.cache.Set(cacheKey, warehouses, policy);
            }

            return warehouses;
        }

        public IEnumerable<Warehouse> GetNearestWarehousesTo(string city, string province, string country)
        {
            string cacheKey = "nearestToCity" + city;

            var warehouses = cache[cacheKey] as IEnumerable<Warehouse>;

            if (warehouses == null)
            {
                warehouses = FindNearestWarehousesTo(city, province, country);

                // set the result in cache
                CacheItemPolicy policy = new CacheItemPolicy();
                // doesn't change very often, so set expiration time to 1 day
                policy.SlidingExpiration = new TimeSpan(1, 0, 0, 0);
                this.cache.Set(cacheKey, warehouses, policy);
            }

            return warehouses;
        }

        public IEnumerable<Warehouse> GetWarehousesByCity(string city)
        {
            string cacheKey = "warehousesIn" + city;

            var warehouses = this.cache[cacheKey] as IEnumerable<Warehouse>;

            if (warehouses == null)
            {
                warehouses = this.warehouseRepo.GetWarehousesByCity(city);

                CacheItemPolicy policy = new CacheItemPolicy();
                // doesn't change very often, so set expiration time to 1 day
                policy.SlidingExpiration = new TimeSpan(1, 0, 0, 0);
                this.cache.Set(cacheKey, warehouses, policy);
            }

            return warehouses;
        }

        public Warehouse GetWarehouseByName(string name)
        {
            string cacheKey = name + "Warehouse";

            var warehouse = this.cache[cacheKey] as Warehouse;

            if (warehouse == null)
            {
                warehouse = this.warehouseRepo.GetWarehouse(name);

                CacheItemPolicy policy = new CacheItemPolicy();
                // almost doesn't change, so set expiration time to 1 week
                policy.SlidingExpiration = new TimeSpan(7, 0, 0, 0);
                this.cache.Set(cacheKey, warehouse, policy);
            }

            return warehouse;
        }

        public Warehouse GetWarehouseByID(int id)
        {
            string cacheKey = "Warehouse" + id.ToString();

            var warehouse = this.cache[cacheKey] as Warehouse;

            if (warehouse == null)
            {
                warehouse = this.warehouseRepo.GetWarehouse(id);

                CacheItemPolicy policy = new CacheItemPolicy();
                // almost doesn't change, so set expiration time to 1 week
                policy.SlidingExpiration = new TimeSpan(7, 0, 0, 0);
                this.cache.Set(cacheKey, warehouse, policy);
            }

            return warehouse;
        }

        public void AddRegion(Region region)
        {
            regionRepo.AddRegion(region);
        }

        public void AddWarehouse(Warehouse warehouse)
        {
            warehouseRepo.AddWarehouse(warehouse);
        }

        public void UpdateWarehouse(Warehouse warehouse)
        {
            warehouseRepo.UpdateWarehouse(warehouse);
        }

        public void RemoveWarehouse(Warehouse warehouse)
        {
            warehouseRepo.RemoveWarehouse(warehouse);
        }

        public void Dispose()
        {
            // clean up resources

            if (this.regionRepo != null)
                regionRepo.Dispose();

            if (this.warehouseRepo != null)
                warehouseRepo.Dispose();
        }

        private IEnumerable<Warehouse> FindNearestWarehousesTo(Warehouse warehouse)
        {
            IEnumerable<Warehouse> result =
                FindNearestWarehousesTo(warehouse.City, warehouse.Province,
                                        warehouse.Country, warehouse.Region);

            // remove the warehouse itself from the result
            List<Warehouse> tempList = new List<Warehouse>(result);
            var warehouseObject = tempList.Find(w => w.Name == warehouse.Name);
            tempList.Remove(warehouseObject);
            result = tempList;

            return result;
        }

        private IEnumerable<Warehouse> FindNearestWarehousesTo(string city, string province, string country, Region region = null)
        {
            IEnumerable<Warehouse> result =
                this.warehouseRepo.GetWarehousesByCity(city);

            if (result == null)
            {
                result = this.warehouseRepo.GetWarehousesByProvince(province);

                if (result == null)
                {
                    if (region == null)
                        region = regionRepo.GetRegionByProvince(country, province);

                    result = this.warehouseRepo.GetWarehousesByRegion(region.Name);
                }
            }

            return result;
        }
    }
}