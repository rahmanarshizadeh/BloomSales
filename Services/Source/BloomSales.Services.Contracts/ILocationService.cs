using BloomSales.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BloomSales.Services.Contracts
{
    [ServiceContract]
    public interface ILocationService
    {
        [OperationContract]
        IEnumerable<Province> GetAllProvinces(string country);

        [OperationContract]
        IEnumerable<Region> GetAllRegions(string country);

        [OperationContract]
        IEnumerable<Warehouse> GetWarehousesByRegion(string region);

        [OperationContract(Name = "GetNearestWarehousesByWarehouse")]
        IEnumerable<Warehouse> GetNearestWarehousesTo(Warehouse warehouse);

        [OperationContract(Name = "GetNearestWarehousesByCity")]
        IEnumerable<Warehouse> GetNearestWarehousesTo(string city, string province, string country);

        [OperationContract]
        IEnumerable<Warehouse> GetWarehousesByCity(string city);

        [OperationContract]
        Warehouse GetWarehouseByName(string name);

        [OperationContract]
        Warehouse GetWarehouseByID(int id);

        [OperationContract]
        void AddRegion(Region region);

        [OperationContract]
        void AddWarehouse(Warehouse warehouse);

        [OperationContract]
        void UpdateWarehouse(Warehouse warehouse);

        [OperationContract]
        void RemoveWarehouse(Warehouse warehouse);
    }
}