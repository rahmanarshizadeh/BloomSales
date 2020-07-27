using BloomSales.Data.Entities;
using BloomSales.Data.Repositories;
using BloomSales.Services.Contracts;
using System;
using System.Runtime.Caching;
using System.ServiceModel;

namespace BloomSales.Services
{
    [ServiceBehavior(UseSynchronizationContext = false,
                     ConcurrencyMode = ConcurrencyMode.Multiple,
                     InstanceContextMode = InstanceContextMode.PerCall)]
    public class AccountingService : IAccountingService, IDisposable
    {
        private ObjectCache cache;
        private IPaymentInfoRepository paymentRepo;
        private ISalesTaxRepository taxRepo;

        public AccountingService()
        {
            this.paymentRepo = new PaymentInfoRepository();
            this.taxRepo = new SalesTaxRepository();
            this.cache = MemoryCache.Default;
        }

        public AccountingService(IPaymentInfoRepository paymentRepository,
                                 SalesTaxRepository taxRepository, ObjectCache cache)
        {
            this.paymentRepo = paymentRepository;
            this.taxRepo = taxRepository;
            this.cache = cache;
        }

        public void AddTaxInfo(SalesTaxInfo taxInfo)
        {
            taxRepo.AddTaxInfo(taxInfo);
        }

        public void Dispose()
        {
            if (this.paymentRepo != null)
                paymentRepo.Dispose();

            if (this.taxRepo != null)
                taxRepo.Dispose();
        }

        public PaymentInfo GetPaymentFor(int orderID)
        {
            string cacheKey = "paymentFor" + orderID.ToString();
            PaymentInfo result = cache[cacheKey] as PaymentInfo;

            if (result == null)
            {
                result = paymentRepo.GetPayment(orderID);
                CacheItemPolicy policy = new CacheItemPolicy();
                // probably it's not going to change at all, but it's not going to
                // be asked for often either. so, set the expiration to 20 minutes
                policy.SlidingExpiration = new TimeSpan(0, 20, 0);
                cache.Set(cacheKey, result, policy);
            }

            return result;
        }

        public SalesTaxInfo GetTaxInfo(string country, string province)
        {
            string cacheKey = "taxFor" + province + country;

            SalesTaxInfo result = cache[cacheKey] as SalesTaxInfo;

            if (result == null)
            {
                result = taxRepo.GetTaxInfo(country, province);
                cache.Set(cacheKey, result, CachingPolicies.OneDayPolicy);
            }

            return result;
        }

        public bool ProcessPayment(PaymentInfo payment)
        {
            // assume that the process has been successfully processed.

            payment.ReceivedDate = DateTime.Now;
            payment.IsReceived = true;

            this.paymentRepo.AddPayment(payment);

            return true;
        }

        public void UpdateTaxInfo(SalesTaxInfo taxInfo)
        {
            taxRepo.UpdateTaxInfo(taxInfo);
        }
    }
}