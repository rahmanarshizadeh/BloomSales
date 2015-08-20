using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BloomSales.Data.Entities;
using System.ServiceModel;

namespace BloomSales.Services.Contracts
{
    [ServiceContract]
    public interface ILocationService
    {
        IEnumerable<Region> GetAllRegions(string country);

        string[] GetCitiesInRegion(string region);

        IEnumerable<Warehouse> GetWarehousesInRegion(string region);

        IEnumerable<Warehouse> GetNearestWarehouses(Warehouse warehouse);

        IEnumerable<Warehouse> GetWarehousesInCity(string city);

        IEnumerable<Warehouse> GetNearestWarehouses(string city);

        Warehouse GetWarehouseByName(string name);

        Warehouse GetWarehouseByID(int id);

        void AddRegion(Region region);

        void AddWarehouse(Warehouse warehouse);

        void UpdateWarehouse(Warehouse warehouse);

        void RemoveWarehouse(Warehouse warehouse);
    }
}
