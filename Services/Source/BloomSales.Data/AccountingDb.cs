using BloomSales.Data.Entities;
using System.Data.Entity;

namespace BloomSales.Data
{
    internal class AccountingDb : DbContext
    {
        public AccountingDb() : base("name = AccountingDatabase")
        {
            Database.SetInitializer<AccountingDb>(new AccountingDbInitializer());
        }

        public virtual DbSet<PaymentInfo> Payments { get; set; }
    }
}