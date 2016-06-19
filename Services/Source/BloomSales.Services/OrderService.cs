using BloomSales.Data.Entities;
using BloomSales.Data.Repositories;
using BloomSales.Services.Contracts;
using BloomSales.Services.Proxies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.ServiceModel;

namespace BloomSales.Services
{
    [ServiceBehavior(UseSynchronizationContext = false,
                     ConcurrencyMode = ConcurrencyMode.Multiple,
                     InstanceContextMode = InstanceContextMode.PerCall)]
    public class OrderService : IOrderService, IDisposable
    {
        private ILocationService locationService;
        private IShippingService shippingService;
        private IAccountingService accountingService;
        private IInventoryService inventoryService;
        private IOrderRepository orderRepo;
        private IOrderItemRepository orderItemRepo;
        private ObjectCache cache;

        public OrderService()
        {
            this.locationService = new LocationClient();
            this.shippingService = new ShippingClient();
            this.accountingService = new AccountingClient();
            this.inventoryService = new InventoryClient();
            this.orderRepo = new OrderRepository();
            this.orderItemRepo = new OrderItemRepository();
            this.cache = MemoryCache.Default;
        }

        public OrderService(ILocationService srvcLocation,
                            IShippingService srvcShipping,
                            IAccountingService srvcAccounting,
                            IInventoryService srvcInventory,
                            IOrderRepository orderRepository,
                            IOrderItemRepository orderItemRepository,
                            ObjectCache cache)
        {
            this.locationService = srvcLocation;
            this.shippingService = srvcShipping;
            this.accountingService = srvcAccounting;
            this.inventoryService = srvcInventory;
            this.orderRepo = orderRepository;
            this.orderItemRepo = orderItemRepository;
            this.cache = cache;
        }

        public bool PlaceOrder(Order order, ShippingInfo shipping, PaymentInfo payment)
        {
            order.HasProcessed = false;
            int orderID = orderRepo.AddOrder(order);

            // update order ID wherever needed
            order.ID = orderID;
            payment.OrderID = orderID;
            shipping.OrderID = orderID;
            foreach (OrderItem item in order.Items)
                item.OrderID = orderID;

            if (!accountingService.ProcessPayment(payment))
                return false;

            var orderSegments = SegmentOrderOnAvailability(order.Items, shipping);

            order.HasProcessed = true;

            orderRepo.UpdateOrder(order);

            Warehouse pickupLocation;

            if (orderSegments.Count > 1)
                pickupLocation = PlaceSuborders(orderSegments, order.ID);
            else
                pickupLocation = orderSegments.First().Key;

            shipping.PickupLocation = pickupLocation;
            shipping.WarehouseID = pickupLocation.ID;
            this.shippingService.RequestShipping(shipping);

            return true;
        }

        public Order GetOrder(int id)
        {
            string cacheKey = "order#" + id.ToString();

            var result = cache[cacheKey] as Order;

            if (result == null)
            {
                result = this.orderRepo.GetOrder(id);
                this.cache.Set(cacheKey, result, CachingPolicies.OneDayPolicy);
            }

            return result;
        }

        public IEnumerable<Order> GetOrderHistoryByCustomer(int customerID, DateTime startDate, DateTime endDate)
        {
            string cacheKey = "c#" + customerID.ToString() + "Orders(" + startDate.ToString() + "-" + endDate.ToString() + ")";

            var result = this.cache[cacheKey] as IEnumerable<Order>;

            if (result == null)
            {
                result = this.orderRepo.GetOrdersByCustomer(customerID, startDate, endDate);
                this.cache.Set(cacheKey, result, CachingPolicies.OneDayPolicy);
            }

            return result;
        }

        public void Dispose()
        {
            if (this.orderRepo != null)
                orderRepo.Dispose();

            if (this.orderItemRepo != null)
                orderItemRepo.Dispose();
        }

        private IDictionary<Warehouse, IEnumerable<OrderItem>> SegmentOrderOnAvailability(
            IEnumerable<OrderItem> itemsList, ShippingInfo shipping)
        {
            List<OrderItem> items = new List<OrderItem>(itemsList);

            Dictionary<Warehouse, IEnumerable<OrderItem>> orderSegments =
                new Dictionary<Warehouse, IEnumerable<OrderItem>>();

            var warehouses =
                locationService.GetNearestWarehousesTo(shipping.City,
                                                       shipping.Province,
                                                       shipping.Country);

            foreach (Warehouse w in warehouses)
            {
                if (items.Count > 0)
                    CheckOrderItems(items, w, orderSegments);
                else
                    // no other item is left
                    break;
            }

            return orderSegments;
        }

        private void CheckOrderItems(List<OrderItem> itemsList, Warehouse warehouse,
                                     Dictionary<Warehouse, IEnumerable<OrderItem>> orderSegments)
        {
            List<OrderItem> availableItems = new List<OrderItem>();

            for (int i = 0; i < itemsList.Count;)
            {
                var inventoryItem =
                    this.inventoryService.GetStockByWarehouse(warehouse.Name, itemsList[i].ProductID);

                if (inventoryItem != null && inventoryItem.UnitsInStock > itemsList[i].Quantity)
                {
                    availableItems.Add(itemsList[i]);
                    this.orderItemRepo.AddItem(itemsList[i]);
                    this.inventoryService.UpdateStock(inventoryItem.ID,
                        (short)(inventoryItem.UnitsInStock - itemsList[i].Quantity));
                    itemsList.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }

            if (availableItems.Count > 0)
                orderSegments.Add(warehouse, availableItems);
        }

        private Warehouse PlaceSuborders(IDictionary<Warehouse, IEnumerable<OrderItem>> orderSegments,
                                    int parentOrderID)
        {
            var keys = orderSegments.Keys;
            var services = this.shippingService.GetServicesByShipper("BloomSales");
            int serviceID = services.First().ID;

            Warehouse destination = keys.First();

            for (int i = 1; i < keys.Count; i++)
            {
                var warehouse = keys.ElementAt(i);
                var items = orderSegments[warehouse];

                Order suborder = CreateSuborder(parentOrderID, items);
                suborder.ID = this.orderRepo.AddOrder(suborder);

                ShippingInfo shipping = CreateShipping(destination, warehouse, suborder.ID, serviceID);
                this.shippingService.RequestShipping(shipping);
            }

            return destination;
        }

        private static ShippingInfo CreateShipping(ContactInfo destination, Warehouse warehouse,
                                                   int orderID, int serviceID)
        {
            ShippingInfo shipping = new ShippingInfo()
            {
                City = destination.City,
                Country = destination.Country,
                Email = destination.Email,
                Name = destination.Name,
                OrderID = orderID,
                Phone = destination.Phone,
                PickupLocation = warehouse,
                PostalCode = destination.PostalCode,
                Province = destination.Province,
                ServiceID = serviceID,
                Status = ShippingStatus.None,
                StreetAddress = destination.StreetAddress,
                WarehouseID = warehouse.ID
            };
            return shipping;
        }

        private static Order CreateSuborder(int parentOrderID, IEnumerable<OrderItem> items)
        {
            Order suborder = new Order()
            {
                IsInternalOrder = true,
                CustomerID = -1,
                ParentOrderID = parentOrderID,
                Items = items,
                OrderDate = DateTime.Now
            };
            return suborder;
        }
    }
}