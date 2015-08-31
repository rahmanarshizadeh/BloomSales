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
    public class ProductCategoryRepositoryTests
    {
        [TestMethod]
        public void GetAllCategories_OnNotEmptyTable_ReturnsAllRecords()
        {
            // arrange
            List<ProductCategory> expected = new List<ProductCategory>();
            expected.Add(new ProductCategory() { ID = 1 });
            expected.Add(new ProductCategory() { ID = 2 });
            expected.Add(new ProductCategory() { ID = 3 });
            expected.Add(new ProductCategory() { ID = 4 });
            expected.Add(new ProductCategory() { ID = 5 });
            Mock<DbSet<ProductCategory>> mockSet = EntityMockFactory.CreateSet(expected.AsQueryable());
            Mock<InventoryDb> mockContext = new Mock<InventoryDb>();
            mockContext.Setup(c => c.Categories).Returns(mockSet.Object);
            ProductCategoryRepository sut = new ProductCategoryRepository(mockContext.Object);

            // act
            var actual = sut.GetAllCategories();

            // assert
            Assert.IsTrue(Equality.AreEqual(expected, actual));
        }

        [TestMethod]
        public void GetCategory_GivenAValidName_ReturnsTheCategory()
        {
            // arrange
            List<ProductCategory> data = new List<ProductCategory>();
            data.Add(new ProductCategory() { Name = "Food" });
            data.Add(new ProductCategory() { Name = "Beverage" });
            data.Add(new ProductCategory() { Name = "Stationary" });
            data.Add(new ProductCategory() { Name = "Health" });
            ProductCategory expected = data[2];
            Mock<DbSet<ProductCategory>> mockSet = EntityMockFactory.CreateSet(data.AsQueryable());
            Mock<InventoryDb> mockContext = new Mock<InventoryDb>();
            mockContext.Setup(c => c.Categories).Returns(mockSet.Object);
            ProductCategoryRepository sut = new ProductCategoryRepository(mockContext.Object);

            // act
            ProductCategory actual = sut.GetCategory("Stationary");

            // assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetCategoryID_GivenAValidName_ReturnsTheID()
        {
            // arrange
            List<ProductCategory> data = new List<ProductCategory>();
            data.Add(new ProductCategory() { ID = 1, Name = "Food" });
            data.Add(new ProductCategory() { ID = 2, Name = "Beverage" });
            data.Add(new ProductCategory() { ID = 3, Name = "Stationary" });
            data.Add(new ProductCategory() { ID = 4, Name = "Health" });
            int expected = data[1].ID;
            Mock<DbSet<ProductCategory>> mockSet = EntityMockFactory.CreateSet(data.AsQueryable());
            Mock<InventoryDb> mockContext = new Mock<InventoryDb>();
            mockContext.Setup(c => c.Categories).Returns(mockSet.Object);
            ProductCategoryRepository sut = new ProductCategoryRepository(mockContext.Object);

            // act
            int actual = sut.GetCategoryID("Beverage");

            // assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AddCategory_GivenANewCategory_AddsToDatabase()
        {
            // arrange
            ProductCategory category = new ProductCategory() { ID = 12 };
            Mock<DbSet<ProductCategory>> mockSet = new Mock<DbSet<ProductCategory>>();
            Mock<InventoryDb> mockContext = new Mock<InventoryDb>();
            mockContext.Setup(c => c.Categories).Returns(mockSet.Object);
            ProductCategoryRepository sut = new ProductCategoryRepository(mockContext.Object);

            // act
            sut.AddCategory(category);

            // assert
            mockSet.Verify(s => s.Add(It.Is<ProductCategory>(c => c.ID == 12)), Times.Once());
            mockContext.Verify(c => c.SaveChanges(), Times.Once());
        }
    }
}
