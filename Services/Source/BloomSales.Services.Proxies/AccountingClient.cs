using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using BloomSales.Services.Contracts;
using BloomSales.Data.Entities;

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
    }
}
