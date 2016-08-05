using BloomSales.Data.Entities;
using BloomSales.Data.Repositories;
using BloomSales.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BloomSales.Services
{
    [ServiceBehavior(UseSynchronizationContext = false,
                     ConcurrencyMode = ConcurrencyMode.Multiple,
                     InstanceContextMode = InstanceContextMode.PerCall)]
    public class ShippingService : IShippingService, IDisposable
    {
        private IShippingInfoRepository shippingRepo;
        private IDeliveryServiceRepository serviceRepo;
        private IShipperRepository shipperRepo;
        private ObjectCache cache;

        public ShippingService()
        {
            this.shipperRepo = new ShipperRepository();
            this.shippingRepo = new ShippingInfoRepository();
            this.serviceRepo = new DeliveryServiceRepository();
            this.cache = MemoryCache.Default;
        }

        public ShippingService(IShipperRepository shipperRepository,
                               IShippingInfoRepository shippingRepository,
                               IDeliveryServiceRepository serviceRepository,
                               ObjectCache cache)
        {
            this.shippingRepo = shippingRepository;
            this.shipperRepo = shipperRepository;
            this.serviceRepo = serviceRepository;
            this.cache = cache;
        }

        public IEnumerable<Shipper> GetAllShippers()
        {
            string cacheKey = "allShippers";
            IEnumerable<Shipper> result = cache[cacheKey] as IEnumerable<Shipper>;

            if (result == null)
            {
                result = this.shipperRepo.GetAllShippers();
                CacheItemPolicy policy = new CacheItemPolicy();
                // set the expiration time to 12 hours
                policy.SlidingExpiration = new TimeSpan(12, 0, 0);
                cache.Set(cacheKey, result, policy);
            }

            return result;
        }

        public IEnumerable<DeliveryService> GetServicesByShipper(string shipper)
        {
            string cacheKey = "servicesBy" + shipper;
            IEnumerable<DeliveryService> result = cache[cacheKey] as IEnumerable<DeliveryService>;

            if (result == null)
            {
                Shipper shipperRecord = this.shipperRepo.GetShipper(shipper);
                result = serviceRepo.GetServicesByShipper(shipperRecord.ID);
                CacheItemPolicy policy = new CacheItemPolicy();
                // set the expiration time to 12 hours
                policy.SlidingExpiration = new TimeSpan(12, 0, 0);
                cache.Set(cacheKey, result, policy);
            }

            return result;
        }

        public void RequestShipping(ShippingInfo shipping)
        {
            shipping.Status = ShippingStatus.ReceivedOrder;
            this.shippingRepo.AddShipping(shipping);
        }

        public ShippingStatus GetShippingStatus(int orderID)
        {
            string cacheKey = "statusForOrder#" + orderID.ToString();
            object status = cache[cacheKey];

            if (status == null)
            {
                status = this.shippingRepo.GetShippingStatus(orderID);
                CacheItemPolicy policy = new CacheItemPolicy();
                // set the expiration time to 12 hours
                policy.SlidingExpiration = new TimeSpan(12, 0, 0);
                cache.Set(cacheKey, status, policy);
            }

            return (ShippingStatus)status;
        }

        public ShippingInfo GetShipping(int orderID)
        {
            string cacheKey = "shippingForOrder#" + orderID.ToString();

            var shipping = cache[cacheKey] as ShippingInfo;

            if (shipping == null)
            {
                shipping = shippingRepo.GetShipping(orderID);

                cache.Set(cacheKey, shipping, CachingPolicies.ThirtyMinutesPolicy);
            }

            return shipping;
        }

        public void AddShipper(Shipper shipper)
        {
            this.shipperRepo.AddShipper(shipper);
        }

        public void AddDeliveryService(DeliveryService service)
        {
            this.serviceRepo.AddService(service);
        }

        public void Dispose()
        {
            if (this.shipperRepo != null)
                shippingRepo.Dispose();

            if (this.shipperRepo != null)
                shipperRepo.Dispose();

            if (this.serviceRepo != null)
                serviceRepo.Dispose();
        }
    }
}