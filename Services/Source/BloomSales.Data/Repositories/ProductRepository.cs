using BloomSales.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BloomSales.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private InventoryDb db;

        public ProductRepository()
        {
            this.db = new InventoryDb();
        }

        internal ProductRepository(InventoryDb context)
        {
            this.db = context;
        }

        public void AddProduct(Product product)
        {
            this.db.Products.Add(product);
            this.db.SaveChanges();
        }

        public void Dispose()
        {
            if (this.db != null)
                db.Dispose();
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return this.db.Products.ToArray();
        }

        public Product GetProduct(int productID)
        {
            return this.db.Products.Find(productID);
        }

        public IEnumerable<Product> GetProducts(int categoryID)
        {
            var result = (from p in db.Products
                          where p.CategoryID == categoryID
                          select p).ToArray();

            return result;
        }
    }
}