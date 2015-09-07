namespace BloomSales.Data.Migrations
{
    using BloomSales.Data.Entities;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class ShippingDbConfiguration : DbMigrationsConfiguration<ShippingDb>
    {
        public ShippingDbConfiguration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ShippingDb context)
        {
            if (context.Shippers.Count() == 0)
                AddShippers(context);

            if (context.Services.Count() == 0)
                AddServices(context);
        }

        private void AddShippers(ShippingDb context)
        {
            var shippers = new List<Shipper>()
            {
                new Shipper()
                { 
                    Name = "BloomSales",
                    Email = "shippings@ca.bloomsales.com",
                    Phone = "1-416-779-1103",
                    StreetAddress = "1023 Lakeshore Blvd. East",
                    City = "Toronto",
                    Province = "ON",
                    PostalCode = "M4M 1B6",
                    Country = "Canada"
                },
                new Shipper()
                {
                    Name = "UPS",
                    Email = "help@ups.com",
                    Phone = "1-800-742-5877",
                    StreetAddress = "3401 NW 67th Avenue, Bld. 805",
                    City = "Miami",
                    Province = "FL",
                    PostalCode = "33122",
                    Country = "USA"
                },
                new Shipper()
                {
                    Name = "Canada Post",
                    Email = "help@canadapost.ca",
                    Phone = "1-866-607-6301",
                    StreetAddress = "2701 Riverside Drive",
                    City = "Ottawa",
                    Province = "ON",
                    PostalCode = "K1V 1J8",
                    Country = "Canada"
                },
                new Shipper()
                {
                    Name = "DHL",
                    Email = "customeraccounting.help@dhl.com",
                    Phone = "1-855-345-7447",
                    StreetAddress = "18 Parkshore Drive",
                    City = "Brampton",
                    Province = "ON",
                    PostalCode = "L6T 5M1",
                    Country = "Canada"
                },
                new Shipper()
                {
                    Name = "Fedex",
                    Email = "help@fedex.com",
                    Phone = "1-800-463-3339",
                    StreetAddress = "3875 Airways, Module H3 Department 4634",
                    City = "Memphis",
                    Province = "TN",
                    PostalCode = "38116",
                    Country = "USA"
                }
            };

            context.Shippers.AddRange(shippers);
            context.SaveChanges();
        }

        private void AddServices(ShippingDb context)
        {
            var services = new List<DeliveryService>()
            {
                new DeliveryService()
                {
                    ServiceName = "Internal Shipping",
                    ShipperID = context.Shippers.Single(s => s.Name == "BloomSales").ID,
                    Cost = 0M
                },
                new DeliveryService()
                {
                    ServiceName = "DHL Express",
                    ShipperID = context.Shippers.Single(s => s.Name == "DHL").ID,
                    Cost = 70M
                },
                new DeliveryService()
                {
                    ServiceName = "DHL Global",
                    ShipperID = context.Shippers.Single(s => s.Name == "DHL").ID,
                    Cost = 150M
                },
                new DeliveryService()
                {
                    ServiceName = "Xpresspost",
                    ShipperID = context.Shippers.Single(s => s.Name == "Canada Post").ID,
                    Cost = 9M
                },
                new DeliveryService()
                {
                    ServiceName = "UPS Express Critical",
                    ShipperID = context.Shippers.Single(s => s.Name == "UPS").ID,
                    Cost = 30M
                },
                new DeliveryService()
                {
                    ServiceName = "UPS Next Day Air",
                    ShipperID = context.Shippers.Single(s => s.Name == "UPS").ID,
                    Cost = 20M
                },
                new DeliveryService()
                {
                    ServiceName = "UPS Ground",
                    ShipperID = context.Shippers.Single(s => s.Name == "UPS").ID,
                    Cost = 10M
                },
                new DeliveryService()
                {
                    ServiceName = "FedEx International First",
                    ShipperID = context.Shippers.Single(s => s.Name == "FedEx").ID,
                    Cost = 150M
                },
                new DeliveryService()
                {
                    ServiceName = "FedEx Ground",
                    ShipperID = context.Shippers.Single(s => s.Name == "FedEx").ID,
                    Cost = 20M
                }
            };

            context.Services.AddRange(services);
            context.SaveChanges();
        }
    }
}
