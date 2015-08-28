using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using BloomSales.Services.Contracts;
using BloomSales.Data.Entities;

namespace BloomSales.Services.Proxies
{
    public class LocationClient : ClientBase<ILocationService>, ILocationService
    {
        public IEnumerable<Region> GetAllRegions(string country)
        {
            return Channel.GetAllRegions(country);
        }

        public IEnumerable<Warehouse> GetWarehousesByRegion(string region)
        {
            return Channel.GetWarehousesByRegion(region);
        }

        public IEnumerable<Warehouse> GetNearestWarehousesTo(Warehouse warehouse)
        {
            return Channel.GetNearestWarehousesTo(warehouse);
        }

        public IEnumerable<Warehouse> GetWarehousesByCity(string city)
        {
            return Channel.GetWarehousesByCity(city);
        }

        public Warehouse GetWarehouseByName(string name)
        {
            return Channel.GetWarehouseByName(name);
        }

        public Warehouse GetWarehouseByID(int id)
        {
            return Channel.GetWarehouseByID(id);
        }

        public void AddRegion(Region region)
        {
            Channel.AddRegion(region);
        }

        public void AddWarehouse(Warehouse warehouse)
        {
            Channel.AddWarehouse(warehouse);
        }

        public void UpdateWarehouse(Warehouse warehouse)
        {
            Channel.UpdateWarehouse(warehouse);
        }

        public void RemoveWarehouse(Warehouse warehouse)
        {
            Channel.RemoveWarehouse(warehouse);
        }
    }
}
