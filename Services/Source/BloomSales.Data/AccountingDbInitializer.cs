using System.Data.Entity;

namespace BloomSales.Data
{
    internal class AccountingDbInitializer : CreateDatabaseIfNotExists<AccountingDb>
    {
        protected override void Seed(AccountingDb context)
        {
            base.Seed(context);
        }
    }
}