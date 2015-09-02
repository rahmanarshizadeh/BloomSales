using BloomSales.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using BloomSales.Data.Entities;

namespace BloomSales.Services.Proxies
{
    public class InventoryClient : ClientBase<IInventoryService>, IInventoryService
    {
        public IEnumerable<Product> GetAllProducts()
        {
            return Channel.GetAllProducts();
        }

        public IEnumerable<Product> GetProductsByCategory(string categoryName)
        {
            return Channel.GetProductsByCategory(categoryName);
        }

        public IEnumerable<ProductCategory> GetCategories()
        {
            return Channel.GetCategories();
        }

        public IEnumerable<InventoryItem> GetInventoryByCity(string city)
        {
            return Channel.GetInventoryByCity(city);
        }

        public IEnumerable<InventoryItem> GetInventoryByWarehouse(string warehouse)
        {
            return Channel.GetInventoryByWarehouse(warehouse);
        }

        public IEnumerable<InventoryItem> GetInventoryByRegion(string region)
        {
            return Channel.GetInventoryByRegion(region);
        }

        public IEnumerable<InventoryItem> GetStocksByCity(string city, int productID)
        {
            return Channel.GetStocksByCity(city, productID);
        }

        public IEnumerable<InventoryItem> GetStocksByRegion(string region, int productID)
        {
            return Channel.GetStocksByRegion(region, productID);
        }

        public InventoryItem GetStockByWarehouse(string warehouse, int productID)
        {
            return Channel.GetStockByWarehouse(warehouse, productID);
        }

        public void AddProduct(Product product)
        {
            Channel.AddProduct(product);
        }

        public void AddToInventory(InventoryItem item)
        {
            Channel.AddToInventory(item);
        }

        public void UpdateStock(int inventoryItemID, short newStock)
        {
            Channel.UpdateStock(inventoryItemID, newStock);
        }

        public void AddCategory(ProductCategory category)
        {
            Channel.AddCategory(category);
        }
    }
}
