using BloomSales.Data.Entities;
using System.Collections.Generic;
using System.ServiceModel;

namespace BloomSales.Services.Contracts
{
    [ServiceContract]
    public interface IInventoryService
    {
        [OperationContract]
        IEnumerable<Product> GetAllProducts();

        [OperationContract]
        IEnumerable<Product> GetProductsByCategory(string categoryName);

        [OperationContract]
        IEnumerable<Product> GetProductsByIDs(IEnumerable<int> productsIDs);

        [OperationContract]
        Product GetProductByID(int productID);

        [OperationContract]
        IEnumerable<ProductCategory> GetCategories();

        [OperationContract]
        IEnumerable<InventoryItem> GetInventoryByCity(string city);

        [OperationContract]
        IEnumerable<InventoryItem> GetInventoryByWarehouse(string warehouse);

        [OperationContract]
        IEnumerable<InventoryItem> GetInventoryByRegion(string region);

        [OperationContract]
        IEnumerable<InventoryItem> GetStocksByCity(string city, int productID);

        [OperationContract]
        IEnumerable<InventoryItem> GetStocksByRegion(string region, int productID);

        [OperationContract]
        InventoryItem GetStockByWarehouse(string warehouse, int productID);

        [OperationContract]
        void AddProduct(Product product);

        [OperationContract]
        void AddToInventory(InventoryItem item);

        [OperationContract]
        void UpdateStock(int inventoryItemID, short newStock);

        [OperationContract]
        void AddCategory(ProductCategory category);
    }
}