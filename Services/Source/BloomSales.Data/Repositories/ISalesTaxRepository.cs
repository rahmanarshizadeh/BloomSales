using BloomSales.Data.Entities;

namespace BloomSales.Data.Repositories
{
    public interface ISalesTaxRepository : IRepository
    {
        SalesTaxInfo GetTaxInfo(string country, string province);

        void AddTaxInfo(SalesTaxInfo salesTax);

        void UpdateTaxInfo(SalesTaxInfo salesTax);
    }
}