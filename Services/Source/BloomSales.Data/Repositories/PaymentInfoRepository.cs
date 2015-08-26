using BloomSales.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloomSales.Data.Repositories
{
    public class PaymentInfoRepository : IPaymentInfoRepository
    {
        private AccountingDb db;

        public PaymentInfoRepository()
        {
            this.db = new AccountingDb();
        }

        internal PaymentInfoRepository(AccountingDb context)
        {
            this.db = context;
        }

        public void AddPayment(PaymentInfo payment)
        {
            db.Payments.Add(payment);
            db.SaveChanges();
        }

        public PaymentInfo GetPayment(int orderID)
        {
            var result = (from p in db.Payments
                          where p.OrderID == orderID
                          select p).SingleOrDefault();

            return result;
        }

        public void Dispose()
        {
            if (this.db != null)
                db.Dispose();
        }
    }
}
