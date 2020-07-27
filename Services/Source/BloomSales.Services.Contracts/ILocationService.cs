using BloomSales.Data.Entities;
using System.Collections.Generic;
using System.ServiceModel;

namespace BloomSales.Services.Contracts
{
    [ServiceContract]
    public interface ILocationService
    {
        [OperationContract]
        void AddRegion(Region region);

        [OperationContract]
        void AddWarehouse(Warehouse warehouse);

        [OperationContract]
        IEnumerable<Province> GetAllProvinces(string country);

        [OperationContract]
        IEnumerable<Region> GetAllRegions(string country);

        [OperationContract(Name = "GetNearestWarehousesByWarehouse")]
        IEnumerable<Warehouse> GetNearestWarehousesTo(Warehouse warehouse);

        [OperationContract(Name = "GetNearestWarehousesByCity")]
        IEnumerable<Warehouse> GetNearestWarehousesTo(string city, string province, string country);

        [OperationContract]
        Warehouse GetWarehouseByID(int id);

        [OperationContract]
        Warehouse GetWarehouseByName(string name);

        [OperationContract]
        IEnumerable<Warehouse> GetWarehousesByCity(string city);

        [OperationContract]
        IEnumerable<Warehouse> GetWarehousesByRegion(string region);

        [OperationContract]
        void RemoveWarehouse(Warehouse warehouse);

        [OperationContract]
        void UpdateWarehouse(Warehouse warehouse);
    }
}