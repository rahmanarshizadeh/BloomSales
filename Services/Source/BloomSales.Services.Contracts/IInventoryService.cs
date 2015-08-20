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

        IEnumerable<Product> GetProductsByCategory(int categoryID);

        IEnumerable<ProductCategory> GetCategories();

        IEnumerable<InventoryItem> GetInventoryByCity(string city);

        IEnumerable<InventoryItem> GetInventoryByWarehouse(string warehouse);

        IEnumerable<InventoryItem> GetInventoryByRegion(string region);

        IEnumerable<InventoryItem> GetStockByCity(string city, int productID);

        IEnumerable<InventoryItem> GetStockByRegion(string region, int productID);

        InventoryItem GetStockByWarehouse(string warehouse, int productID);

        void AddToInventory(InventoryItem item);

        void UpdateInventory(InventoryItem item);

        void AddCategory(ProductCategory category);
    }
}
