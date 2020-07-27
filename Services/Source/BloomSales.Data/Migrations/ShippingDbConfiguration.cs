namespace BloomSales.Data.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class ShippingDbConfiguration : DbMigrationsConfiguration<ShippingDb>
    {
        public ShippingDbConfiguration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ShippingDb context)
        {
            // do nothing!
        }
    }
}