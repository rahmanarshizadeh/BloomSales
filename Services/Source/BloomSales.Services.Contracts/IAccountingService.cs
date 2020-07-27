using BloomSales.Data.Entities;
using System.ServiceModel;

namespace BloomSales.Services.Contracts
{
    [ServiceContract]
    public interface IAccountingService
    {
        [OperationContract]
        void AddTaxInfo(SalesTaxInfo taxInfo);

        [OperationContract]
        PaymentInfo GetPaymentFor(int orderID);

        [OperationContract]
        SalesTaxInfo GetTaxInfo(string country, string province);

        [OperationContract]
        bool ProcessPayment(PaymentInfo payment);

        [OperationContract]
        void UpdateTaxInfo(SalesTaxInfo taxInfo);
    }
}