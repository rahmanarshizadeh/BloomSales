namespace BloomSales.Data.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class LocationDbConfiguration : DbMigrationsConfiguration<BloomSales.Data.LocationDb>
    {
        public LocationDbConfiguration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BloomSales.Data.LocationDb context)
        {
            // nothing!
        }
    }
}