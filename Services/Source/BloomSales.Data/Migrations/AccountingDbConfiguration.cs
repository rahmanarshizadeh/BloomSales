namespace BloomSales.Data.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class AccountingDbConfiguration : DbMigrationsConfiguration<BloomSales.Data.AccountingDb>
    {
        public AccountingDbConfiguration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BloomSales.Data.AccountingDb context)
        {
            // nothing!
        }
    }
}