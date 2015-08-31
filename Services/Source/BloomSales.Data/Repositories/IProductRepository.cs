using BloomSales.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloomSales.Data.Repositories
{
    public interface IProductRepository : IRepository
    {
        IEnumerable<Product> GetAllProducts();

        IEnumerable<Product> GetProducts(int categoryID);

        Product GetProduct(int productID);

        void AddProduct(Product product);
    }
}
