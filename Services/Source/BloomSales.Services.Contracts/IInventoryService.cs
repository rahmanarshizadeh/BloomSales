using BloomSales.Data.Entities;
using System.Collections.Generic;
using System.ServiceModel;

namespace BloomSales.Services.Contracts
{
    [ServiceContract]
    public interface IInventoryService
    {
        [OperationContract]
        void AddCategory(ProductCategory category);

        [OperationContract]
        void AddProduct(Product product);

        [OperationContract]
        void AddToInventory(InventoryItem item);

        [OperationContract]
        IEnumerable<Product> GetAllProducts();

        [OperationContract]
        IEnumerable<ProductCategory> GetCategories();

        [OperationContract]
        IEnumerable<InventoryItem> GetInventoryByCity(string city);

        [OperationContract]
        IEnumerable<InventoryItem> GetInventoryByRegion(string region);

        [OperationContract]
        IEnumerable<InventoryItem> GetInventoryByWarehouse(string warehouse);

        [OperationContract]
        Product GetProductByID(int productID);

        [OperationContract]
        IEnumerable<Product> GetProductsByCategory(string categoryName);

        [OperationContract]
        IEnumerable<Product> GetProductsByIDs(IEnumerable<int> productsIDs);

        [OperationContract]
        InventoryItem GetStockByWarehouse(string warehouse, int productID);

        [OperationContract]
        IEnumerable<InventoryItem> GetStocksByCity(string city, int productID);

        [OperationContract]
        IEnumerable<InventoryItem> GetStocksByRegion(string region, int productID);

        [OperationContract]
        void UpdateStock(int inventoryItemID, short newStock);
    }
}