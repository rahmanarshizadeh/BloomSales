using BloomSales.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BloomSales.Services.Contracts
{
    [ServiceContract]
    public interface IAccountingService
    {
        bool ProcessPayment(PaymentInfo payment);

        PaymentInfo GetPaymentFor(int orderID);
    }
}
