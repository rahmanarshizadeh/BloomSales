﻿using BloomSales.Data.Entities;
using BloomSales.Data.Repositories;
using BloomSales.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace BloomSales.Data.Tests.Repositories
{
    [TestClass]
    public class PaymentInfoRepositoryTests
    {
        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        public void AddPayment_GivenANewPayment_AddsToDatabase()
        {
            // arrange
            PaymentInfo payment = new PaymentInfo { OrderID = 10 };
            Mock<DbSet<PaymentInfo>> mockSet = new Mock<DbSet<PaymentInfo>>();
            Mock<AccountingDb> mockContext = new Mock<AccountingDb>();
            mockContext.Setup(c => c.Payments).Returns(mockSet.Object);
            PaymentInfoRepository sut = new PaymentInfoRepository(mockContext.Object);

            // act
            sut.AddPayment(payment);

            // assert
            mockSet.Verify(s => s.Add(It.Is<PaymentInfo>(p => p.OrderID == payment.OrderID)), Times.Once());
            mockContext.Verify(c => c.SaveChanges(), Times.Once());
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        public void GetPeyment_GivenAValidOrderID_ReturnsThePaymentRecord()
        {
            // arrange
            List<PaymentInfo> data = new List<PaymentInfo>();
            PaymentInfo expected = new PaymentInfo() { OrderID = 210 };
            data.Add(expected);
            Mock<DbSet<PaymentInfo>> mockSet = EntityMockFactory.CreateSet(data.AsQueryable());
            Mock<AccountingDb> mockContext = new Mock<AccountingDb>();
            mockContext.Setup(c => c.Payments).Returns(mockSet.Object);
            PaymentInfoRepository sut = new PaymentInfoRepository(mockContext.Object);

            // act
            PaymentInfo actual = sut.GetPayment(210);

            // assert
            Assert.AreEqual(expected, actual);
        }
    }
}