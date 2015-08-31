using BloomSales.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloomSales.Services.Contracts
{
    public interface IInventoryService
    {
        IEnumerable<Product> GetAllProducts();

        IEnumerable<Product> GetProductsByCategory(string categoryName);

        IEnumerable<ProductCategory> GetCategories();

        IEnumerable<InventoryItem> GetInventoryByCity(string city);

        IEnumerable<InventoryItem> GetInventoryByWarehouse(string warehouse);

        IEnumerable<InventoryItem> GetInventoryByRegion(string region);

        IEnumerable<InventoryItem> GetStocksByCity(string city, int productID);

        IEnumerable<InventoryItem> GetStocksByRegion(string region, int productID);

        InventoryItem GetStockByWarehouse(string warehouse, int productID);

        void AddProduct(Product product);
        
        void AddToInventory(InventoryItem item);

        void UpdateStock(int inventoryItemID, short newStock);

        void AddCategory(ProductCategory category);
    }
}
