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
        [OperationContract]
        bool ProcessPayment(PaymentInfo payment);

        [OperationContract]
        PaymentInfo GetPaymentFor(int orderID);

        [OperationContract]
        void AddTaxInfo(SalesTaxInfo taxInfo);

        [OperationContract]
        SalesTaxInfo GetTaxInfo(string country, string province);

        [OperationContract]
        void UpdateTaxInfo(SalesTaxInfo taxInfo);
    }
}