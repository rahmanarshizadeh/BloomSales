using System.Data.Entity;

namespace BloomSales.Data
{
    internal class ShippingDbInitializer : CreateDatabaseIfNotExists<ShippingDb>
    {
        protected override void Seed(ShippingDb context)
        {
            base.Seed(context);
        }
    }
}