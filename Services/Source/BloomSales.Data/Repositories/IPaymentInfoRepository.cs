using BloomSales.Data.Entities;

namespace BloomSales.Data.Repositories
{
    public interface IPaymentInfoRepository : IRepository
    {
        void AddPayment(PaymentInfo payment);

        PaymentInfo GetPayment(int orderID);
    }
}