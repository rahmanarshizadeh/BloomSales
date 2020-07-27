using BloomSales.Data.Entities;
using BloomSales.Services.Proxies;
using BloomSales.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;
using System.ServiceModel;

namespace BloomSales.Services.IntegrationTests
{
    [TestClass]
    public class AccountingServiceTests
    {
        private static ServiceHost accountingSvcHost;
        private AccountingClient accountingClient;

        [ClassCleanup]
        public static void CleanupService()
        {
            accountingSvcHost.Close();

            // drop the Shipping Database
            Database.Delete("AccountingDatabase");
        }

        [ClassInitialize]
        public static void InitializeService(TestContext textContext)
        {
            // drop the Shipping Database if it exists
            if (Database.Exists("AccountingDatabase"))
                Database.Delete("AccountingDatabase");

            // spin up Shipping Service
            accountingSvcHost = new ServiceHost(typeof(AccountingService));
            accountingSvcHost.Open();
        }

        [TestCleanup]
        public void CleanupClient()
        {
            this.accountingClient.Close();
        }

        [TestInitialize]
        public void InitializeClient()
        {
            accountingClient = new AccountingClient();
            accountingClient.Open();
        }

        [TestMethod]
        [TestCategory(TestType.IntegrationTest)]
        [TestCategory("BloomSales.Services.IntegrationTests.AccountingServiceTests")]
        public void ProcessPayment_GivenANewPayment_ProcessesThePayment()
        {
            // arrange
            var payment = new PaymentInfo()
            {
                Amount = 123,
                Currency = "CAD",
                OrderID = 77,
                Type = PaymentType.OnlineBanking
            };

            // act
            var actualResult = accountingClient.ProcessPayment(payment);

            // assert
            var actualPayment = accountingClient.GetPaymentFor(77);
            Assert.IsTrue(actualResult);
            Assert.AreEqual(123, actualPayment.Amount);
            Assert.AreEqual("CAD", actualPayment.Currency);
            Assert.AreEqual(PaymentType.OnlineBanking, actualPayment.Type);
            Assert.IsTrue(actualPayment.IsReceived);
        }
    }
}