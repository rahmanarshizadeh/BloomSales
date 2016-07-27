using BloomSales.Data.Entities;
using BloomSales.Data.Repositories;
using BloomSales.Services.Contracts;
using BloomSales.Services.Proxies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BloomSales.Services
{
    [ServiceBehavior(UseSynchronizationContext = false,
                    ConcurrencyMode = ConcurrencyMode.Multiple,
                    InstanceContextMode = InstanceContextMode.PerCall)]
    public class InventoryService : IInventoryService, IDisposable
    {
        private ObjectCache cache;
        private IInventoryItemRepository inventoryRepo;
        private IProductCategoryRepository categoryRepo;
        private IProductRepository productRepo;
        private ILocationService locationService;
        private CacheItemPolicy oneDayPolicy;
        private CacheItemPolicy twelveHoursPolicy;
        private CacheItemPolicy oneHourPolicy;
        private CacheItemPolicy thirtyMinutesPolicy;
        private CacheItemPolicy tenMinutesPolicy;

        public InventoryService()
        {
            this.cache = MemoryCache.Default;
            this.inventoryRepo = new InventoryItemRepository();
            this.categoryRepo = new ProductCategoryRepository();
            this.productRepo = new ProductRepository();
            this.locationService = new LocationClient();
            InitializePolicies();
        }

        public InventoryService(IInventoryItemRepository inventoryRepository,
                                IProductCategoryRepository categoryRepository,
                                IProductRepository prodcutRepository,
                                ObjectCache cache,
                                ILocationService locationService)
        {
            this.cache = cache;
            this.inventoryRepo = inventoryRepository;
            this.productRepo = prodcutRepository;
            this.categoryRepo = categoryRepository;
            this.locationService = locationService;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            string cacheKey = "allProducts";

            var result = this.cache[cacheKey] as IEnumerable<Product>;

            if (result == null)
            {
                result = this.productRepo.GetAllProducts();
                this.cache.Set(cacheKey, result, this.oneDayPolicy);
            }

            return result;
        }

        public IEnumerable<Product> GetProductsByCategory(string categoryName)
        {
            string cacheKey = "allProductsIn" + categoryName + "Category";

            var result = this.cache[cacheKey] as IEnumerable<Product>;

            if (result == null)
            {
                ProductCategory category = this.categoryRepo.GetCategory(categoryName);
                result = this.productRepo.GetProducts(category.ID);

                foreach (var item in result)
                    item.Category = category;

                this.cache.Set(cacheKey, result, this.oneDayPolicy);
            }

            return result;
        }

        public IEnumerable<Product> GetProductsByIDs(IEnumerable<int> productIDs)
        {
            string cacheKey = string.Empty;
            Product p;
            List<Product> result = new List<Product>();

            foreach (int id in productIDs)
            {
                cacheKey = string.Format("product#{0}", id);

                p = cache[cacheKey] as Product;

                if (p == null)
                {
                    p = productRepo.GetProduct(id);

                    cache.Set(cacheKey, p, this.oneDayPolicy);
                }

                result.Add(p);
            }

            return result;
        }

        public IEnumerable<ProductCategory> GetCategories()
        {
            string cacheKey = "allCategories";

            var result = this.cache[cacheKey] as IEnumerable<ProductCategory>;

            if (result == null)
            {
                result = this.categoryRepo.GetAllCategories();
                this.cache.Set(cacheKey, result, this.oneDayPolicy);
            }

            return result;
        }

        public IEnumerable<InventoryItem> GetInventoryByCity(string city)
        {
            string cacheKey = "inventoryIn" + city;

            var result = this.cache[cacheKey] as IEnumerable<InventoryItem>;

            if (result == null)
            {
                IEnumerable<Warehouse> warehouses = locationService.GetWarehousesByCity(city);
                result = this.inventoryRepo.GetInventories(warehouses);
                this.cache.Set(cacheKey, result, this.thirtyMinutesPolicy);
            }

            return result;
        }

        public IEnumerable<InventoryItem> GetInventoryByWarehouse(string warehouse)
        {
            string cacheKey = "inventoryIn" + warehouse + "Warehouse";

            var result = this.cache[cacheKey] as IEnumerable<InventoryItem>;

            if (result == null)
            {
                Warehouse w = this.locationService.GetWarehouseByName(warehouse);
                result = this.inventoryRepo.GetInventory(w);
                this.cache.Set(cacheKey, result, this.thirtyMinutesPolicy);
            }

            return result;
        }

        public IEnumerable<InventoryItem> GetInventoryByRegion(string region)
        {
            string cacheKey = "inventoryIn" + region + "Region";

            var result = this.cache[cacheKey] as IEnumerable<InventoryItem>;

            if (result == null)
            {
                var warehouses = this.locationService.GetWarehousesByRegion(region);
                result = this.inventoryRepo.GetInventories(warehouses);
                this.cache.Set(cacheKey, result, this.tenMinutesPolicy);
            }

            return result;
        }

        public IEnumerable<InventoryItem> GetStocksByCity(string city, int productID)
        {
            string cacheKey = "stocksOfP" + productID.ToString() + "In" + city;

            var result = this.cache[cacheKey] as IEnumerable<InventoryItem>;

            if (result == null)
            {
                var warehouses = this.locationService.GetWarehousesByCity(city);
                result = this.inventoryRepo.GetStocks(warehouses, productID);
                this.cache.Set(cacheKey, result, this.thirtyMinutesPolicy);
            }

            return result;
        }

        public IEnumerable<InventoryItem> GetStocksByRegion(string region, int productID)
        {
            string cacheKey = "stocksOfP" + productID.ToString() + "In" + region + "Region";

            var result = this.cache[cacheKey] as IEnumerable<InventoryItem>;

            if (result == null)
            {
                var warehouses = this.locationService.GetWarehousesByRegion(region);
                result = this.inventoryRepo.GetStocks(warehouses, productID);
                this.cache.Set(cacheKey, result, this.tenMinutesPolicy);
            }

            return result;
        }

        public InventoryItem GetStockByWarehouse(string warehouse, int productID)
        {
            string cacheKey = "stockOfP" + productID.ToString() + "In" + warehouse + "Warehouse";

            var result = this.cache[cacheKey] as InventoryItem;

            if (result == null)
            {
                Warehouse w = this.locationService.GetWarehouseByName(warehouse);
                result = this.inventoryRepo.GetStock(w, productID);
                this.cache.Set(cacheKey, result, this.oneHourPolicy);
            }

            return result;
        }

        public void AddProduct(Product product)
        {
            this.productRepo.AddProduct(product);

            // remove the cache record for "All Products"
            if (cache["allProducts"] != null)
                cache.Remove("allProducts");
        }

        public void AddToInventory(InventoryItem item)
        {
            this.inventoryRepo.AddToInventory(item);
        }

        public void UpdateStock(int inventoryItemID, short newStock)
        {
            this.inventoryRepo.UpdateStock(inventoryItemID, newStock);
        }

        public void AddCategory(ProductCategory category)
        {
            this.categoryRepo.AddCategory(category);

            // remove the cache record for "All Categories"
            if (cache["allCategories"] != null)
                cache.Remove("allCategories");
        }

        public void Dispose()
        {
            if (this.categoryRepo != null)
                categoryRepo.Dispose();

            if (this.inventoryRepo != null)
                inventoryRepo.Dispose();

            if (this.productRepo != null)
                productRepo.Dispose();
        }

        private void InitializePolicies()
        {
            this.oneDayPolicy = new CacheItemPolicy() { SlidingExpiration = new TimeSpan(1, 0, 0, 0) };
            this.twelveHoursPolicy = new CacheItemPolicy() { SlidingExpiration = new TimeSpan(12, 0, 0) };
            this.oneHourPolicy = new CacheItemPolicy() { SlidingExpiration = new TimeSpan(1, 0, 0) };
            this.thirtyMinutesPolicy = new CacheItemPolicy() { SlidingExpiration = new TimeSpan(0, 30, 0) };
            this.tenMinutesPolicy = new CacheItemPolicy() { SlidingExpiration = new TimeSpan(0, 10, 0) };
        }
    }
}