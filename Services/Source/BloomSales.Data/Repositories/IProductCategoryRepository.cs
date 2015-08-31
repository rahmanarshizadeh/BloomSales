using BloomSales.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloomSales.Data.Repositories
{
    public interface IProductCategoryRepository : IRepository
    {
        IEnumerable<ProductCategory> GetAllCategories();

        ProductCategory GetCategory(string name);

        int GetCategoryID(string name);

        void AddCategory(ProductCategory category);
    }
}
