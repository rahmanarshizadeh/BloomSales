using BloomSales.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BloomSales.Web.Store.Models
{
    public class BillViewModel
    {
        public Order Order { get; set; }

        public ShippingInfo Shipping { get; set; }

        public SalesTaxInfo Tax { get; set; }

        public decimal ShippingCost { get; set; }

        public PaymentInfo Payment { get; set; }

        public decimal OrderSubtotal
        {
            get { return CalculateSubtotal(); }
        }

        public decimal OrderTotal
        {
            get { return CalculateTotal(); }
        }

        private decimal CalculateSubtotal()
        {
            decimal result = 0;

            foreach (var item in Order.Items)
                result += item.Quantity * item.UnitPrice;

            return result;
        }

        private decimal CalculateTotal()
        {
            var subtotal = CalculateSubtotal();

            var totalTax = Tax.Federal + Tax.Provincial;

            var result = subtotal + ShippingCost;
            result += (result * (decimal)totalTax);

            return result;
        }
    }
}