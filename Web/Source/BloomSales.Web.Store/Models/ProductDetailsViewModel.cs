using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BloomSales.Web.Store.Models
{
    public class ProductDetailsViewModel
    {
        public ProductDetailsViewModel()
        {
            // do nothing!
        }

        public ProductDetailsViewModel(int id, decimal unitPrice)
        {
            ID = id;
            UnitPrice = unitPrice;
        }

        public int ID { get; set; }

        public decimal UnitPrice { get; set; }
    }
}