using BloomSales.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloomSales.Data
{
    internal class AccountingDb : DbContext
    {
        public AccountingDb() : base("name = AccountingDatabase")
        {
            // do nothing!
        }

        public virtual DbSet<PaymentInfo> Payments { get; set; }
    }
}
