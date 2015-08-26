using BloomSales.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloomSales.Data.Repositories
{
    public interface IPaymentInfoRepository : IRepository
    {
        void AddPayment(PaymentInfo payment);

        PaymentInfo GetPayment(int orderID);
    }
}
