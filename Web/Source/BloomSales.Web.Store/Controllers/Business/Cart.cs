using BloomSales.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BloomSales.Web.Store.Controllers.Business
{
    public class Cart
    {
        private Order order;
        private List<OrderItem> items;

        public Cart(Order initialOrder)
        {
            if (initialOrder != null)
                order = initialOrder;
            else
                order = new Order();

            if (order.Items != null)
                items = new List<OrderItem>(order.Items);
            else
                items = new List<OrderItem>();
        }

        public Order Order
        {
            get
            {
                order.Items = items;
                return order;
            }
        }

        public int NumberOfItems
        {
            get { return items.Count; }
        }

        public decimal Subtotal
        {
            get { return CalculateSubtotal(); }
        }

        public IEnumerable<OrderItem> Items
        {
            get { return items; }
        }

        public IEnumerable<int> ItemsProductIDs
        {
            get { return GetProductIDs(); }
        }

        public void AddItem(OrderItem item)
        {
            var duplicate = items.Find(i => i.ProductID == item.ProductID);

            if (duplicate != null)
                duplicate.Quantity++;
            else
                items.Add(item);
        }

        public void Update(IEnumerable<OrderItem> itemsList)
        {
            List<OrderItem> newList = new List<OrderItem>();

            foreach (OrderItem item in itemsList)
                // remove unnecessary items (qantity = 0)
                if (item.Quantity > 0)
                    newList.Add(item);

            items = newList;
        }

        private decimal CalculateSubtotal()
        {
            decimal subtotal = 0;

            foreach (var item in items)
                subtotal += item.Quantity * item.UnitPrice;

            return subtotal;
        }

        private IEnumerable<int> GetProductIDs()
        {
            List<int> productIDs = new List<int>();

            foreach (var item in items)
                productIDs.Add(item.ProductID);

            return productIDs;
        }
    }
}