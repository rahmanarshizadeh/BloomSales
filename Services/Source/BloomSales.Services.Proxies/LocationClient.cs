using BloomSales.Data.Entities;
using BloomSales.Services.Contracts;
using System.Collections.Generic;
using System.ServiceModel;

namespace BloomSales.Services.Proxies
{
    public class LocationClient : ClientBase<ILocationService>, ILocationService
    {
        public void AddRegion(Region region)
        {
            Channel.AddRegion(region);
        }

        public void AddWarehouse(Warehouse warehouse)
        {
            Channel.AddWarehouse(warehouse);
        }

        public IEnumerable<Province> GetAllProvinces(string country)
        {
            return Channel.GetAllProvinces(country);
        }

        public IEnumerable<Region> GetAllRegions(string country)
        {
            return Channel.GetAllRegions(country);
        }

        public IEnumerable<Warehouse> GetNearestWarehousesTo(Warehouse warehouse)
        {
            return Channel.GetNearestWarehousesTo(warehouse);
        }

        public IEnumerable<Warehouse> GetNearestWarehousesTo(string city, string province, string country)
        {
            return Channel.GetNearestWarehousesTo(city, province, country);
        }

        public Warehouse GetWarehouseByID(int id)
        {
            return Channel.GetWarehouseByID(id);
        }

        public Warehouse GetWarehouseByName(string name)
        {
            return Channel.GetWarehouseByName(name);
        }

        public IEnumerable<Warehouse> GetWarehousesByCity(string city)
        {
            return Channel.GetWarehousesByCity(city);
        }

        public IEnumerable<Warehouse> GetWarehousesByRegion(string region)
        {
            return Channel.GetWarehousesByRegion(region);
        }

        public void RemoveWarehouse(Warehouse warehouse)
        {
            Channel.RemoveWarehouse(warehouse);
        }

        public void UpdateWarehouse(Warehouse warehouse)
        {
            Channel.UpdateWarehouse(warehouse);
        }
    }
}