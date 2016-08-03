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

        public decimal OrderSubtotal { get; set; }

        public decimal ShippingCost { get; set; }

        public PaymentInfo Payment { get; set; }
    }
}