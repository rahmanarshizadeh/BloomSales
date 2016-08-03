using BloomSales.Data.Entities;
using BloomSales.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BloomSales.Services.Proxies
{
    public class AccountingClient : ClientBase<IAccountingService>, IAccountingService
    {
        public bool ProcessPayment(PaymentInfo payment)
        {
            return Channel.ProcessPayment(payment);
        }

        public PaymentInfo GetPaymentFor(int orderID)
        {
            return Channel.GetPaymentFor(orderID);
        }

        public void AddTaxInfo(SalesTaxInfo taxInfo)
        {
            Channel.AddTaxInfo(taxInfo);
        }

        public SalesTaxInfo GetTaxInfo(string country, string province)
        {
            return Channel.GetTaxInfo(country, province);
        }

        public void UpdateTaxInfo(SalesTaxInfo taxInfo)
        {
            Channel.UpdateTaxInfo(taxInfo);
        }
    }
}