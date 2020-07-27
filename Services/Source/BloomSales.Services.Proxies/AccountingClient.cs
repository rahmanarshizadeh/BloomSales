using BloomSales.Data.Entities;
using BloomSales.Services.Contracts;
using System.ServiceModel;

namespace BloomSales.Services.Proxies
{
    public class AccountingClient : ClientBase<IAccountingService>, IAccountingService
    {
        public void AddTaxInfo(SalesTaxInfo taxInfo)
        {
            Channel.AddTaxInfo(taxInfo);
        }

        public PaymentInfo GetPaymentFor(int orderID)
        {
            return Channel.GetPaymentFor(orderID);
        }

        public SalesTaxInfo GetTaxInfo(string country, string province)
        {
            return Channel.GetTaxInfo(country, province);
        }

        public bool ProcessPayment(PaymentInfo payment)
        {
            return Channel.ProcessPayment(payment);
        }

        public void UpdateTaxInfo(SalesTaxInfo taxInfo)
        {
            Channel.UpdateTaxInfo(taxInfo);
        }
    }
}