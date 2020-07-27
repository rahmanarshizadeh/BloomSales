using BloomSales.Data.Entities;
using BloomSales.Services.Contracts;
using System.Collections.Generic;
using System.ServiceModel;

namespace BloomSales.Services.Proxies
{
    public class InventoryClient : ClientBase<IInventoryService>, IInventoryService
    {
        public void AddCategory(ProductCategory category)
        {
            Channel.AddCategory(category);
        }

        public void AddProduct(Product product)
        {
            Channel.AddProduct(product);
        }

        public void AddToInventory(InventoryItem item)
        {
            Channel.AddToInventory(item);
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return Channel.GetAllProducts();
        }

        public IEnumerable<ProductCategory> GetCategories()
        {
            return Channel.GetCategories();
        }

        public IEnumerable<InventoryItem> GetInventoryByCity(string city)
        {
            return Channel.GetInventoryByCity(city);
        }

        public IEnumerable<InventoryItem> GetInventoryByRegion(string region)
        {
            return Channel.GetInventoryByRegion(region);
        }

        public IEnumerable<InventoryItem> GetInventoryByWarehouse(string warehouse)
        {
            return Channel.GetInventoryByWarehouse(warehouse);
        }

        public Product GetProductByID(int productID)
        {
            return Channel.GetProductByID(productID);
        }

        public IEnumerable<Product> GetProductsByCategory(string categoryName)
        {
            return Channel.GetProductsByCategory(categoryName);
        }

        public IEnumerable<Product> GetProductsByIDs(IEnumerable<int> productIDs)
        {
            return Channel.GetProductsByIDs(productIDs);
        }

        public InventoryItem GetStockByWarehouse(string warehouse, int productID)
        {
            return Channel.GetStockByWarehouse(warehouse, productID);
        }

        public IEnumerable<InventoryItem> GetStocksByCity(string city, int productID)
        {
            return Channel.GetStocksByCity(city, productID);
        }

        public IEnumerable<InventoryItem> GetStocksByRegion(string region, int productID)
        {
            return Channel.GetStocksByRegion(region, productID);
        }

        public void UpdateStock(int inventoryItemID, short newStock)
        {
            Channel.UpdateStock(inventoryItemID, newStock);
        }
    }
}