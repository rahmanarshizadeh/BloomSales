using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BloomSales.Data.Entities;

namespace BloomSales.Data.Repositories
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private InventoryDb db;

        public ProductCategoryRepository()
        {
            this.db = new InventoryDb();
        }

        internal ProductCategoryRepository(InventoryDb context)
        {
            this.db = context;
        }

        public IEnumerable<ProductCategory> GetAllCategories()
        {
            return this.db.Categories.ToArray();
        }

        public ProductCategory GetCategory(string name)
        {
            var result = (from c in db.Categories
                          where c.Name.Equals(name)
                          select c).Single();

            return result;
        }

        public int GetCategoryID(string name)
        {
            ProductCategory p = GetCategory(name);

            return p.ID;
        }

        public void AddCategory(ProductCategory category)
        {
            this.db.Categories.Add(category);
            this.db.SaveChanges();
        }

        public void Dispose()
        {
            if (this.db != null)
                db.Dispose();
        }
    }
}
