using BloomSales.Data.Entities;
using BloomSales.Data.Repositories;
using BloomSales.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Runtime.Caching;

namespace BloomSales.Services.Tests
{
    [TestClass]
    public class AccountingServiceTests
    {
        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        [TestCategory("BloomSales.Services.Tests.AccountingServiceTests")]
        public void ProcessPayment_GivenANewPayment_ProcessesAndAddsThePaymentToDatabase()
        {
            // arrange
            Mock<IPaymentInfoRepository> mockRepo = new Mock<IPaymentInfoRepository>();
            PaymentInfo payment = new PaymentInfo() { OrderID = 10 };
            AccountingService sut = new AccountingService(mockRepo.Object, null);

            // act
            sut.ProcessPayment(payment);

            // assert
            mockRepo.Verify(
                r => r.AddPayment(It.Is<PaymentInfo>(
                    p => p.OrderID == 10 && p.ReceivedDate != DateTime.MinValue && p.IsReceived == true)),
                    Times.Once());
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        [TestCategory("BloomSales.Services.Tests.AccountingServiceTests")]
        public void GetPaymentFor_ResultExistsInCache_ReturnsThePaymentResult()
        {
            // arrange
            PaymentInfo expected = new PaymentInfo() { OrderID = 10 };
            Mock<ObjectCache> mockCache = new Mock<ObjectCache>();
            mockCache.SetupGet(c => c["paymentFor10"]).Returns(expected);
            AccountingService sut = new AccountingService(null, mockCache.Object);

            // act
            PaymentInfo actual = sut.GetPaymentFor(10);

            // assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory(TestType.UnitTest)]
        [TestCategory("BloomSales.Services.Tests.AccountingServiceTests")]
        public void GetPaymentFor_ResultNotExistsInCache_FetchesTheResultFromDatabaseAndAddsToCache()
        {
            // arrange
            PaymentInfo expected = new PaymentInfo() { OrderID = 200 };
            Mock<ObjectCache> mockCache = new Mock<ObjectCache>();
            Mock<IPaymentInfoRepository> mockRepo = new Mock<IPaymentInfoRepository>();
            mockRepo.Setup(r => r.GetPayment(It.Is<int>(p => p == 200))).Returns(expected);
            AccountingService sut = new AccountingService(mockRepo.Object, mockCache.Object);

            // act
            PaymentInfo actual = sut.GetPaymentFor(200);

            // assert
            Assert.AreEqual(expected, actual);
            mockCache.Verify(
                c => c.Set(
                    It.Is<string>(k => k.Equals("paymentFor200")),
                    It.Is<PaymentInfo>(p => p.OrderID == 200),
                    It.IsAny<CacheItemPolicy>(),
                    null),
                 Times.Once());
        }
    }
}