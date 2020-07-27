using BloomSales.Data.Entities;
using System.Collections.Generic;

namespace BloomSales.Data.Repositories
{
    public interface IWarehouseRepository : IRepository
    {
        void AddWarehouse(Warehouse warehouse);

        Warehouse GetWarehouse(string name);

        Warehouse GetWarehouse(int id);

        IEnumerable<Warehouse> GetWarehousesByCity(string city);

        IEnumerable<Warehouse> GetWarehousesByProvince(string province);

        IEnumerable<Warehouse> GetWarehousesByRegion(string region);

        void RemoveWarehouse(Warehouse warehouse);

        void UpdateWarehouse(Warehouse warehouse);
    }
}