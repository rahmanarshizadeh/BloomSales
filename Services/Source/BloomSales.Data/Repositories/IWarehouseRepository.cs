using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BloomSales.Data.Entities;

namespace BloomSales.Data.Repositories
{
    public interface IWarehouseRepository : IRepository
    {
        IEnumerable<Warehouse> GetWarehousesByRegion(string region);

        IEnumerable<Warehouse> GetWarehousesByCity(string city);

        IEnumerable<Warehouse> GetWarehousesByProvince(string province);

        Warehouse GetWarehouse(string name);

        Warehouse GetWarehouse(int id);

        void AddWarehouse(Warehouse warehouse);

        void UpdateWarehouse(Warehouse warehouse);

        void RemoveWarehouse(Warehouse warehouse);
    }
}
