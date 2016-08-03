using BloomSales.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace BloomSales.Data
{
    internal class AccountingDbInitializer : CreateDatabaseIfNotExists<AccountingDb>
    {
        protected override void Seed(AccountingDb context)
        {
            base.Seed(context);

            AddSalesTaxInfo(context);
        }

        private void AddSalesTaxInfo(AccountingDb context)
        {
            var canadianSalesTaxInfo = new List<SalesTaxInfo>()
            {
                new SalesTaxInfo()
                {
                    Country = "Canada",
                    Province = "Alberta",
                    Federal = 0.05f,
                    Provincial = 0.0f
                },
                new SalesTaxInfo()
                {
                    Country = "Canada",
                    Province = "British Columbia",
                    Federal = 0.05f,
                    Provincial = 0.07f
                },
                new SalesTaxInfo()
                {
                    Country = "Canada",
                    Province = "Manitoba",
                    Federal = 0.05f,
                    Provincial = 0.08f
                },
                new SalesTaxInfo()
                {
                    Country = "Canada",
                    Province = "New Burnswick",
                    Federal = 0.05f,
                    Provincial = 0.10f
                },
                new SalesTaxInfo()
                {
                    Country = "Canada",
                    Province = "Newfoundland and Labrador",
                    Federal = 0.05f,
                    Provincial = 0.08f
                },
                new SalesTaxInfo()
                {
                    Country = "Canada",
                    Province = "Northwest Territories",
                    Federal = 0.05f,
                    Provincial = 0.0f
                },
                new SalesTaxInfo()
                {
                    Country = "Canada",
                    Province = "Nova Scotia",
                    Federal = 0.05f,
                    Provincial = 0.10f
                },
                new SalesTaxInfo()
                {
                    Country = "Canada",
                    Province = "Nunavut",
                    Federal = 0.05f,
                    Provincial = 0.0f
                },
                new SalesTaxInfo()
                {
                    Country = "Canada",
                    Province = "Ontario",
                    Federal = 0.05f,
                    Provincial = 0.08f
                },
                new SalesTaxInfo()
                {
                    Country = "Canada",
                    Province = "Prince Edward Islands",
                    Federal = 0.05f,
                    Provincial = 0.09f
                },
                new SalesTaxInfo()
                {
                    Country = "Canada",
                    Province = "Quebec",
                    Federal = 0.05f,
                    Provincial = 0.0975f
                },
                new SalesTaxInfo()
                {
                    Country = "Canada",
                    Province = "Saskatchewan",
                    Federal = 0.05f,
                    Provincial = 0.05f
                },
                new SalesTaxInfo()
                {
                    Country = "Canada",
                    Province = "Yukon",
                    Federal = 0.05f,
                    Provincial = 0.0f
                }
            };

            context.Taxes.AddRange(canadianSalesTaxInfo);
            context.SaveChanges();
        }
    }
}