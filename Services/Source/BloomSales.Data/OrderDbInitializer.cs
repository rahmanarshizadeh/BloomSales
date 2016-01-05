using System.Data.Entity;

namespace BloomSales.Data
{
    internal class OrderDbInitializer : CreateDatabaseIfNotExists<OrderDb>
    {
        protected override void Seed(OrderDb context)
        {
            base.Seed(context);
        }
    }
}