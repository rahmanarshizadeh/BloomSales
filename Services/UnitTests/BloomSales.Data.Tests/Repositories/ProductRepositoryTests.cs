using BloomSales.Data.Entities;
using BloomSales.Data.Repositories;
using BloomSales.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloomSales.Data.Tests.Repositories
{
    [TestClass]
    public class ProductRepositoryTests
    {
        [TestMethod]
        public void GetAllProducts_OnNonEmptyTable_ReturnsAllProducts()
        {
            // arrange
            List<Product> expected = new List<Product>();
            expected.Add(new Product() { ID = 1 });
            expected.Add(new Product() { ID = 2 });
            expected.Add(new Product() { ID = 3 });
            expected.Add(new Product() { ID = 4 });
            expected.Add(new Product() { ID = 5 });
            Mock<DbSet<Product>> mockSet = EntityMockFactory.CreateSet(expected.AsQueryable());
            Mock<InventoryDb> mockContext = new Mock<InventoryDb>();
            mockContext.Setup(c => c.Products).Returns(mockSet.Object);
            ProductRepository sut = new ProductRepository(mockContext.Object);

            // act
            var actual = sut.GetAllProducts();

            // assert
            Assert.IsTrue(Equality.AreEqual(expected, actual));
        }

        [TestMethod]
        public void GetProduct_GivenAValidProductID_ReturnsTheProduct()
        {
            // arrange
            Product expected = new Product() { ID = 2 };
            Mock<DbSet<Product>> mockSet = new Mock<DbSet<Product>>();
            mockSet.Setup(s => s.Find(2)).Returns(expected);
            Mock<InventoryDb> mockContext = new Mock<InventoryDb>();
            mockContext.Setup(c => c.Products).Returns(mockSet.Object);
            ProductRepository sut = new ProductRepository(mockContext.Object);

            // act
            Product actual = sut.GetProduct(2);

            // assert
            Assert.AreEqual(expected, actual);
            mockSet.Verify(s => s.Find(2), Times.Once());
        }

        [TestMethod]
        public void AddProduct_GivenANewProduct_AddsToDatabase()
        {
            // arrange
            Product product = new Product() { ID = 12 };
            Mock<DbSet<Product>> mockSet = new Mock<DbSet<Product>>();
            Mock<InventoryDb> mockContext = new Mock<InventoryDb>();
            mockContext.Setup(c => c.Products).Returns(mockSet.Object);
            ProductRepository sut = new ProductRepository(mockContext.Object);

            // act
            sut.AddProduct(product);

            // assert
            mockSet.Verify(s => s.Add(It.Is<Product>(p => p.ID == 12)), Times.Once());
            mockContext.Verify(c => c.SaveChanges(), Times.Once());
        }
    }
}
