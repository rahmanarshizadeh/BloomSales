using BloomSales.Data.Entities;
using BloomSales.Data.Repositories;
using BloomSales.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace BloomSales.Data.Tests.Repositories
{
    [TestClass]
    public class SalesTaxRepositoryTests
    {
        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        public void AddTaxInfo_GivenANewTaxInfo_AddsToTheDatabase()
        {
            // arrange
            SalesTaxInfo newTax = new SalesTaxInfo()
            {
                Country = "Canada",
                Province = "Ontario"
            };
            List<SalesTaxInfo> data = new List<SalesTaxInfo>();
            Mock<DbSet<SalesTaxInfo>> mockTaxesSet = EntityMockFactory.CreateSet(data.AsQueryable());
            Mock<AccountingDb> mockContext = new Mock<AccountingDb>();
            mockContext.Setup(c => c.Taxes).Returns(mockTaxesSet.Object);
            SalesTaxRepository sut = new SalesTaxRepository(mockContext.Object);

            // act
            sut.AddTaxInfo(newTax);

            // assert
            mockContext.Verify(c => c.Taxes, Times.Exactly(2));
            mockTaxesSet.Verify(s => s.Add(newTax), Times.Once());
            mockContext.Verify(c => c.SaveChanges(), Times.Once());
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        [ExpectedException(typeof(ArgumentException))]
        public void AddTaxInfo_GivenAnExistingTaxInfo_ThrowsException()
        {
            // arrange
            SalesTaxInfo duplicateTax = new SalesTaxInfo()
            {
                Country = "Canada",
                Province = "Quebec"
            };
            var data = new List<SalesTaxInfo>()
            {
                new SalesTaxInfo()
                {
                    Country = "Canada",
                    Province = "Quebec"
                }
            };
            Mock<DbSet<SalesTaxInfo>> mockTaxesSet = EntityMockFactory.CreateSet(data.AsQueryable());
            Mock<AccountingDb> mockContext = new Mock<AccountingDb>();
            mockContext.Setup(c => c.Taxes).Returns(mockTaxesSet.Object);
            SalesTaxRepository sut = new SalesTaxRepository(mockContext.Object);

            // act
            sut.AddTaxInfo(duplicateTax);

            // assert
            // exception should be thrown
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        public void GetTaxInfo_GivenAnExistingCountryAndProvince_ReturnsRecordFromDatabase()
        {
            // arrange
            var data = new List<SalesTaxInfo>()
            {
                new SalesTaxInfo()
                {
                    Country = "Canada",
                    Province = "Quebec",
                    Federal = 0.05f,
                    Provincial = 0.0975f
                }
            };
            var expected = data[0];
            Mock<DbSet<SalesTaxInfo>> mockTaxesSet = EntityMockFactory.CreateSet(data.AsQueryable());
            Mock<AccountingDb> mockContext = new Mock<AccountingDb>();
            mockContext.Setup(c => c.Taxes).Returns(mockTaxesSet.Object);
            SalesTaxRepository sut = new SalesTaxRepository(mockContext.Object);

            // act
            var actual = sut.GetTaxInfo("Canada", "Quebec");

            // assert
            mockContext.Verify(c => c.Taxes, Times.Once());
            Assert.AreEqual(expected.Federal, actual.Federal);
            Assert.AreEqual(expected.Provincial, actual.Provincial);
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        public void UodateTaxInfo_GivenAnUpdatedInfo_UpdatesRecordInDatabase()
        {
            // arrange
            SalesTaxInfo update = new SalesTaxInfo()
            {
                Country = "Canada",
                Province = "Quebec",
                Federal = 0.05f,
                Provincial = 0.0975f
            };
            var data = new List<SalesTaxInfo>()
            {
                new SalesTaxInfo()
                {
                    Country = "Canada",
                    Province = "Quebec",
                    Federal = 0.12f,
                    Provincial = 0.04f
                }
            };
            var actual = data[0];
            Mock<DbSet<SalesTaxInfo>> mockTaxesSet = EntityMockFactory.CreateSet(data.AsQueryable());
            Mock<AccountingDb> mockContext = new Mock<AccountingDb>();
            mockContext.Setup(c => c.Taxes).Returns(mockTaxesSet.Object);
            SalesTaxRepository sut = new SalesTaxRepository(mockContext.Object);

            // act
            sut.UpdateTaxInfo(update);

            // assert
            mockContext.Verify(c => c.Taxes, Times.Once());
            Assert.AreEqual(0.05f, actual.Federal);
            Assert.AreEqual(0.0975f, actual.Provincial);
        }
    }
}