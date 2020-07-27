using BloomSales.Data.Entities;
using System.Collections.Generic;

namespace BloomSales.Data.Repositories
{
    public interface IProductRepository : IRepository
    {
        void AddProduct(Product product);

        IEnumerable<Product> GetAllProducts();

        Product GetProduct(int productID);

        IEnumerable<Product> GetProducts(int categoryID);
    }
}