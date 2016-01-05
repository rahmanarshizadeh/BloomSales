
namespace BloomSales.Data.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class InventoryDbConfiguration : DbMigrationsConfiguration<InventoryDb>
    {
        public InventoryDbConfiguration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(InventoryDb context)
        {
            // nothing!
        }
    }
}