namespace BloomSales.Data.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class OrderDbConfiguration : DbMigrationsConfiguration<BloomSales.Data.OrderDb>
    {
        public OrderDbConfiguration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BloomSales.Data.OrderDb context)
        {
            // nothing!
        }
    }
}