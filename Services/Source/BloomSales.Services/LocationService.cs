using BloomSales.Services.Contracts;
using BloomSales.Data.Entities;
using BloomSales.Data;
using BloomSales.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;

namespace BloomSales.Services
{
    public class LocationService : ILocationService, IDisposable
    {
        private IRegionRepository regionRepo;
        private IWarehouseRepository warehouseRepo;
        private ObjectCache cache;
        private CacheItemPolicy defaultPolicy;

        public LocationService()
        {
            this.regionRepo = new RegionRepository();
            this.warehouseRepo = new WarehouseRepository();
            this.cache = MemoryCache.Default;

            this.defaultPolicy = new CacheItemPolicy();
            // set the expiration time to 1 minute
            this.defaultPolicy.SlidingExpiration = new TimeSpan(0, 1, 0);
        }

        public LocationService(IRegionRepository regionRepo,
                                IWarehouseRepository warehouseRepo,
                                ObjectCache cache)
        {
            this.regionRepo = regionRepo;
            this.warehouseRepo = warehouseRepo;
            this.cache = cache;
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
                this.warehouseRepo.GetWarehousesByCity(warehouse.City);

            if (result == null)
            {
                result = this.warehouseRepo.GetWarehousesByProvince(warehouse.Province);

                if (result == null)
                {
                    Region region = warehouse.Region;

                    if (region == null)
                        region = this.regionRepo.GetRegion(warehouse.RegionID);

                    result = this.warehouseRepo.GetWarehousesByRegion(region.Name);
                }
            }
            return result;
        }
    }
}
