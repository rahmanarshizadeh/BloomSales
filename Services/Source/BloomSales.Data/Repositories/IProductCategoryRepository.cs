using BloomSales.Data.Entities;
using System.Collections.Generic;

namespace BloomSales.Data.Repositories
{
    public interface IProductCategoryRepository : IRepository
    {
        void AddCategory(ProductCategory category);

        IEnumerable<ProductCategory> GetAllCategories();

        ProductCategory GetCategory(string name);

        int GetCategoryID(string name);
    }
}