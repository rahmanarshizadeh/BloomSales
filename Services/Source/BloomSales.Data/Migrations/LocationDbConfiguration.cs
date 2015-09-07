namespace BloomSales.Data.Migrations
{
    using BloomSales.Data.Entities;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class LocationDbConfiguration : DbMigrationsConfiguration<BloomSales.Data.LocationDb>
    {
        public LocationDbConfiguration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BloomSales.Data.LocationDb context)
        {
            if (context.Regions.Count() == 0)
                AddRegions(context);

            if (context.Provinces.Count() == 0)
                AddProvinces(context);

            if (context.Warehouses.Count() == 0)
                AddWarehouses(context);
        }

        private static void AddRegions(BloomSales.Data.LocationDb context)
        {
            var regions = new List<Region>()
            {
                new Region { Continent = "North America", Country = "Canada", Name = "Eastern Canada" },
                new Region { Continent = "North America", Country = "Canada", Name = "Northern Canada" },
                new Region { Continent = "North America", Country = "Canada", Name = "Western Canada" }
            };

            context.Regions.AddRange(regions);
            context.SaveChanges();
        }

        private static void AddProvinces(BloomSales.Data.LocationDb context)
        {
            var provinces = new List<Province>()
            {
                new Province 
                { 
                    Name = "Alberta", 
                    Abbreviation = "AB",
                    RegionID = context.Regions.Single(r => r.Name == "Western Canada").ID 
                },
                new Province 
                { 
                    Name = "British Columbia", 
                    Abbreviation = "BC",
                    RegionID = context.Regions.Single(r => r.Name == "Western Canada").ID 
                },
                new Province 
                { 
                    Name = "Manitoba",
                    Abbreviation = "MB",
                    RegionID = context.Regions.Single(r => r.Name == "Western Canada").ID 
                },
                new Province 
                { 
                    Name = "New Burnswick", 
                    Abbreviation = "NB",
                    RegionID = context.Regions.Single(r => r.Name == "Eastern Canada").ID 
                },
                new Province 
                { 
                    Name = "Newfoundland and Labrador", 
                    Abbreviation = "NL",
                    RegionID = context.Regions.Single(r => r.Name == "Eastern Canada").ID 
                },
                new Province 
                { 
                    Name = "Nova Scotia", 
                    Abbreviation = "NS",
                    RegionID = context.Regions.Single(r => r.Name == "Eastern Canada").ID 
                },
                new Province 
                { 
                    Name = "Ontario",
                    Abbreviation = "ON",
                    RegionID = context.Regions.Single(r => r.Name == "Eastern Canada").ID 
                },
                new Province 
                { 
                    Name = "Prince Edward Islands",
                    Abbreviation = "PE",
                    RegionID = context.Regions.Single(r => r.Name == "Eastern Canada").ID 
                },
                new Province 
                { 
                    Name = "Quebec",
                    Abbreviation = "QC",
                    RegionID = context.Regions.Single(r => r.Name == "Eastern Canada").ID 
                },
                new Province 
                { 
                    Name = "Saskatchewan", 
                    Abbreviation = "SK",
                    RegionID = context.Regions.Single(r => r.Name == "Western Canada").ID 
                },
                new Province 
                { 
                    Name = "Northwest Teritories", 
                    Abbreviation = "NT",
                    RegionID = context.Regions.Single(r => r.Name == "Northern Canada").ID 
                },
                new Province 
                { 
                    Name = "Nunavut", 
                    Abbreviation = "NU",
                    RegionID = context.Regions.Single(r => r.Name == "Northern Canada").ID 
                },
                new Province 
                { 
                    Name = "Yukon", 
                    Abbreviation = "YT",
                    RegionID = context.Regions.Single(r => r.Name == "Northern Canada").ID 
                }
            };

            context.Provinces.AddRange(provinces);
            context.SaveChanges();
        }

        private void AddWarehouses(LocationDb context)
        {
            var warehouses = new List<Warehouse>()
            {
                new Warehouse 
                {
                    Email = "warehouse1@ca.bloomsales.com",
                    Name = "BloomSales W#1",
                    Phone = "1-514-923-9876",
                    StreetAddress = "1234 Saint Catherine",
                    City = "Montreal",
                    PostalCode = "H3G 2H6",
                    Province = "QC",
                    Country = "Canada",
                    RegionID = context.Regions.Single(r => r.Name == "Eastern Canada").ID
                },
                new Warehouse 
                {
                    Email = "warehouse2@ca.bloomsales.com",
                    Name = "BloomSales W#2",
                    Phone = "1-514-767-8561",
                    StreetAddress = "3281 Rue Wellington",
                    City = "Verdun",
                    PostalCode = "H4G 2T4",
                    Province = "QC",
                    Country = "Canada",
                    RegionID = context.Regions.Single(r => r.Name == "Eastern Canada").ID
                },
                new Warehouse 
                {
                    Email = "warehouse3@ca.bloomsales.com",
                    Name = "BloomSales W#3",
                    Phone = "1-416-925-9692",
                    StreetAddress = "739 Yonge Street",
                    City = "Toronto",
                    PostalCode = "M3W 2H5",
                    Province = "ON",
                    Country = "Canada",
                    RegionID = context.Regions.Single(r => r.Name == "Eastern Canada").ID
                },
                new Warehouse 
                {
                    Email = "warehouse4@ca.bloomsales.com",
                    Name = "BloomSales W#4",
                    Phone = "1-416-778-0103",
                    StreetAddress = "1021 Lakeshore Blvd. East",
                    City = "Toronto",
                    PostalCode = "M4M 1B4",
                    Province = "ON",
                    Country = "Canada",
                    RegionID = context.Regions.Single(r => r.Name == "Eastern Canada").ID
                },
                new Warehouse 
                {
                    Email = "warehouse5@ca.bloomsales.com",
                    Name = "BloomSales W#5",
                    Phone = "1-403-248-6401",
                    StreetAddress = "3519 - 8th Avenue Ne",
                    City = "Calgary",
                    PostalCode = "T2A 5K7",
                    Province = "AB",
                    Country = "Canada",
                    RegionID = context.Regions.Single(r => r.Name == "Western Canada").ID
                },
                new Warehouse 
                {
                    Email = "warehouse6@ca.bloomsales.com",
                    Name = "BloomSales W#6",
                    Phone = "1-514-923-9776",
                    StreetAddress = "2290 Cambie Street",
                    City = "Vancouver",
                    PostalCode = "V5Z 3T8",
                    Province = "BC",
                    Country = "Canada",
                    RegionID = context.Regions.Single(r => r.Name == "Western Canada").ID
                },
                new Warehouse 
                {
                    Email = "warehouse7@ca.bloomsales.com",
                    Name = "BloomSales W#7",
                    Phone = "1-902-422-4923",
                    StreetAddress = "6103 Quinpool Road",
                    City = "Halifax",
                    PostalCode = "B3L 5P7",
                    Province = "NS",
                    Country = "Canada",
                    RegionID = context.Regions.Single(r => r.Name == "Eastern Canada").ID
                }
            };

            context.Warehouses.AddRange(warehouses);
            context.SaveChanges();
        }
    }
}
