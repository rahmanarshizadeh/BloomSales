using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BloomSales.Services.Contracts;
using BloomSales.Data.Repositories;
using BloomSales.Data.Entities;
using System.Runtime.Caching;

namespace BloomSales.Services
{
    public class AccountingService : IAccountingService
    {
        private IPaymentInfoRepository repo;
        private ObjectCache cache;

        public AccountingService()
        {
            this.repo = new PaymentInfoRepository();
            this.cache = MemoryCache.Default;
        }

        public AccountingService(IPaymentInfoRepository repository, ObjectCache cache)
        {
            this.repo = repository;
            this.cache = cache;
        }

        public bool ProcessPayment(PaymentInfo payment)
        {
            // assume that the process has been successfully processed.

            payment.ReceivedDate = DateTime.Now;
            payment.IsReceived = true;

            this.repo.AddPayment(payment);

            return true;
        }

        public PaymentInfo GetPaymentFor(int orderID)
        {
            string cacheKey = "paymentFor" + orderID.ToString();
            PaymentInfo result = cache[cacheKey] as PaymentInfo;

            if (result == null)
            {
                result = repo.GetPayment(orderID);
                CacheItemPolicy policy = new CacheItemPolicy();
                // probably it's not going to change at all, but it's not going to 
                // be asked for often either. so, set the expiration to 20 minutes
                policy.SlidingExpiration = new TimeSpan(0, 20, 0);
                cache.Set(cacheKey, result, policy);
            }

            return result;
        }
    }
}
