using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BloomSales.Data.Entities;

namespace BloomSales.Web.Store.Models
{
    public class OrderDetailsViewModel : OrderViewModelBase
    {
        private BillViewModel bill;
        private IEnumerable<IEnumerable<Product>> products;
        private IEnumerable<ShippingInfo> shippings;

        public OrderDetailsViewModel(BillViewModel bill, IEnumerable<ShippingInfo> shippings,
                                     IEnumerable<IEnumerable<Product>> products)
        {
            this.bill = bill;
            this.shippings = shippings;
            this.products = products;
        }

        public ContactInfo Contact
        {
            get { return bill.Shipping; }
        }

        public float FederalTax
        {
            get { return bill.Tax.Federal * 100; }
        }

        public string PaymentCurrency
        {
            get { return bill.Payment.Currency; }
        }

        public string PaymentMethod
        {
            get { return GetPaymentMethodTitle(); }
        }

        public IEnumerable<IEnumerable<Product>> Products
        {
            get { return products; }
        }

        public float ProvincialTax
        {
            get { return bill.Tax.Provincial * 100; }
        }

        public decimal ShippingCost
        {
            get { return bill.ShippingCost; }
        }

        public IEnumerable<ShippingInfo> Shippings
        {
            get { return shippings; }
        }

        public IEnumerable<string> ShippingStatuses
        {
            get { return GetStatusTitles(); }
        }

        public decimal Subtotal
        {
            get { return bill.OrderSubtotal; }
        }

        public IEnumerable<Order> SubOrders
        {
            get { return bill.Order.SubOrders; }
        }

        public decimal Total
        {
            get { return bill.Payment.Amount; }
        }

        private string GetPaymentMethodTitle()
        {
            switch (bill.Payment.Type)
            {
                case PaymentType.CreditCard:
                    return "Credit Card";

                case PaymentType.OnlineBanking:
                    return "Online Banking";

                default:
                    return bill.Payment.Type.ToString();
            }
        }

        private IEnumerable<string> GetStatusTitles()
        {
            var list = new List<string>();

            foreach (var shipping in shippings)
                list.Add(GetStatusTitle(shipping.Status));

            return list;
        }
    }
}