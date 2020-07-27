using BloomSales.Data.Entities;
using System;
using System.Linq;

namespace BloomSales.Data.Repositories
{
    public class SalesTaxRepository : ISalesTaxRepository
    {
        private AccountingDb db;

        public SalesTaxRepository()
        {
            db = new AccountingDb();
        }

        internal SalesTaxRepository(AccountingDb context)
        {
            db = context;
        }

        public void AddTaxInfo(SalesTaxInfo salesTax)
        {
            var taxtInfo = FindRecord(salesTax.Country, salesTax.Province);

            if (taxtInfo != null)
                throw new ArgumentException("A record for the same country and province already exists.");

            db.Taxes.Add(salesTax);
            db.SaveChanges();
        }

        public void Dispose()
        {
            if (db != null)
                db.Dispose();
        }

        public SalesTaxInfo GetTaxInfo(string country, string province)
        {
            return FindRecord(country, province);
        }

        public void UpdateTaxInfo(SalesTaxInfo salesTax)
        {
            var record = FindRecord(salesTax.Country, salesTax.Province);

            record.Provincial = salesTax.Provincial;
            record.Federal = salesTax.Federal;

            db.SaveChanges();
        }

        private SalesTaxInfo FindRecord(string country, string province)
        {
            var result = (from tax in db.Taxes
                          where (tax.Country == country && tax.Province == province)
                          select tax).SingleOrDefault();

            return result;
        }
    }
}