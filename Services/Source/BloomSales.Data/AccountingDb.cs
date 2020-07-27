using BloomSales.Data.Entities;
using System;
using System.Data.Entity;

namespace BloomSales.Data
{
    internal class AccountingDb : DbContext
    {
        public AccountingDb() : base("name = AccountingDatabase")
        {
            Database.SetInitializer<AccountingDb>(new AccountingDbInitializer());
            this.Configuration.ProxyCreationEnabled = false;
        }

        public virtual DbSet<PaymentInfo> Payments { get; set; }

        public virtual DbSet<SalesTaxInfo> Taxes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Properties<DateTime>()
                .Configure(c => c.HasColumnType("datetime2"));
        }
    }
}