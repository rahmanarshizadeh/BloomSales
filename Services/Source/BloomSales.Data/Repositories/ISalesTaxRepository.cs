using BloomSales.Data.Entities;

namespace BloomSales.Data.Repositories
{
    public interface ISalesTaxRepository : IRepository
    {
        void AddTaxInfo(SalesTaxInfo salesTax);

        SalesTaxInfo GetTaxInfo(string country, string province);

        void UpdateTaxInfo(SalesTaxInfo salesTax);
    }
}