using BloomSales.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace BloomSales.Data
{
    internal class InventoryDbInitializer : CreateDatabaseIfNotExists<InventoryDb>
    {
        protected override void Seed(InventoryDb context)
        {
            base.Seed(context);

            AddCategories(context);

            AddProducts(context);

            AddInventories(context, new LocationDb());
        }

        private void AddCategories(InventoryDb context)
        {
            var mainCategories = new List<ProductCategory>
            {
                new ProductCategory() { Name = "Appliances" },
                new ProductCategory() { Name = "Electronics" },
                new ProductCategory() { Name = "Furniture" }
            };

            context.Categories.AddRange(mainCategories);
            context.SaveChanges();

            var subCategories = new List<ProductCategory>
            {
                new ProductCategory()
                {
                    Name = "Fridges",
                    ParentID = context.Categories.Single(c => c.Name == "Appliances").ID
                },
                new ProductCategory()
                {
                    Name = "Stoves",
                    ParentID = context.Categories.Single(c => c.Name == "Appliances").ID
                },
                new ProductCategory()
                {
                    Name = "Vaccums",
                    ParentID = context.Categories.Single(c => c.Name == "Appliances").ID
                },
                new ProductCategory()
                {
                    Name = "Irons",
                    ParentID = context.Categories.Single(c => c.Name == "Appliances").ID
                },
                new ProductCategory()
                {
                    Name = "Washers & Dryers",
                    ParentID = context.Categories.Single(c => c.Name == "Appliances").ID
                },
                new ProductCategory()
                {
                    Name = "Computers",
                    ParentID = context.Categories.Single(c => c.Name == "Electronics").ID
                },
                new ProductCategory()
                {
                    Name = "Tablets",
                    ParentID = context.Categories.Single(c => c.Name == "Electronics").ID
                },
                new ProductCategory()
                {
                    Name = "TV",
                    ParentID = context.Categories.Single(c => c.Name == "Electronics").ID
                },
                new ProductCategory()
                {
                    Name = "Cameras",
                    ParentID = context.Categories.Single(c => c.Name == "Electronics").ID
                },
                new ProductCategory()
                {
                    Name = "Phones",
                    ParentID = context.Categories.Single(c => c.Name == "Electronics").ID
                },
                new ProductCategory()
                {
                    Name = "Living Room",
                    ParentID = context.Categories.Single(c => c.Name == "Furniture").ID
                },
                new ProductCategory()
                {
                    Name = "Bedroom",
                    ParentID = context.Categories.Single(c => c.Name == "Furniture").ID
                },
                new ProductCategory()
                {
                    Name = "Kids Room",
                    ParentID = context.Categories.Single(c => c.Name == "Furniture").ID
                },
                new ProductCategory()
                {
                    Name = "Office",
                    ParentID = context.Categories.Single(c => c.Name == "Furniture").ID
                },
            };

            context.Categories.AddRange(subCategories);
            context.SaveChanges();
        }

        private void AddProducts(InventoryDb context)
        {
            var products = new List<Product>
            {
                new Product()
                {
                    Name = "Kenmore®/MD 30\" Self-Clean Smooth-Top Range, Stainless Steel",
                    Producer = "Kenmore",
                    CategoryID = context.Categories.Single(c => c.Name == "Stoves").ID,
                    UnitPrice = 879.99M,
                    ImageUrl = "http://www.sears.ca/wcsstore/MasterCatalog/images/catalog/Product_271/std_lang_all/23/_p/622_60323_P.jpg"
                },
                new Product()
                {
                    Name = "Kenmore®/MD 30\" Freestanding Self-Clean Convection Electric Range, Stainless Steel",
                    Producer = "Kenmore",
                    CategoryID = context.Categories.Single(c => c.Name == "Stoves").ID,
                    UnitPrice = 999.99M,
                    ImageUrl = "http://www.sears.ca/wcsstore/MasterCatalog/images/catalog/Product_271/std_lang_all/73/_p/622_50373_P.jpg"
                },
                new Product()
                {
                    Name = "Kenmore®/MD 18.2 Cu.Ft Top Mount Refrigerator - White",
                    Producer = "Kenmore",
                    CategoryID = context.Categories.Single(c => c.Name == "Fridges").ID,
                    UnitPrice = 849.99M,
                    ImageUrl = "http://www.sears.ca/wcsstore/MasterCatalog/images/catalog/Product_271/std_lang_all/22/_p/646_42422_P.jpg"
                },
                new Product()
                {
                    Name = "LG 5.0 cu.Ft. Front Load Washer- White",
                    Producer = "LG",
                    CategoryID = context.Categories.Single(c => c.Name == "Washers & Dryers").ID,
                    UnitPrice = 839.91M,
                    ImageUrl = "http://www.sears.ca/wcsstore/MasterCatalog/images/catalog/Product_271/std_lang_all/01/_p/626_25501_P.jpg"
                },
                new Product()
                {
                    Name = "LG 5.0 cu. Ft. Full Size All-in-One 27\" Front Load Washer / Dryer Combo, TurboWash™ & Steam Technology - White",
                    Producer = "LG",
                    CategoryID = context.Categories.Single(c => c.Name == "Washers & Dryers").ID,
                    UnitPrice = 2299.91M,
                    ImageUrl = "http://www.sears.ca/wcsstore/MasterCatalog/images/catalog/Product_271/std_lang_all/21/_p/626_25021_P.jpg"
                },
                new Product()
                {
                    Name = "Kenmore®/MD 12-Amp Bagless Upright Vacuum",
                    Producer = "Kenmore",
                    CategoryID = context.Categories.Single(c => c.Name == "Vaccums").ID,
                    UnitPrice = 139.99M,
                    ImageUrl = "http://www.sears.ca/wcsstore/MasterCatalog/images/catalog/Product_271/std_lang_all/15/_p/620_30615_P.jpg"
                },
                new Product()
                {
                    Name = "Kenmore®/MD Canister Bagged Vacuum",
                    Producer = "Kenmore",
                    CategoryID = context.Categories.Single(c => c.Name == "Vaccums").ID,
                    UnitPrice = 419.99M,
                    ImageUrl = "http://www.sears.ca/wcsstore/MasterCatalog/images/catalog/Product_271/std_lang_all/19/_p/620_23119_P.jpg"
                },
                new Product()
                {
                    Name = "AOC International E2752VH 27 inch Wide LCD Glossy Black",
                    Producer = "AOC International",
                    CategoryID = context.Categories.Single(c => c.Name == "Computers").ID,
                    UnitPrice = 370.99M,
                    ImageUrl = "http://searsca.scene7.com/is/image/searsca/657-000879189-E2752VH?wid=152&hei=152"
                },
                new Product()
                {
                    Name = "Lenovo H50 PC",
                    Producer = "Lenovo",
                    CategoryID = context.Categories.Single(c => c.Name == "Computers").ID,
                    UnitPrice = 499.99M,
                    ImageUrl = "http://www.bestbuy.ca/multimedia/Products/150x150/103/10360/10360967.jpg"
                },
                new Product()
                {
                    Name = "Acer Aspire X PC",
                    Producer = "Acer",
                    CategoryID = context.Categories.Single(c => c.Name == "Computers").ID,
                    UnitPrice = 499.99M,
                    ImageUrl = "http://www.bestbuy.ca/multimedia/Products/150x150/103/10381/10381266.jpg"
                },
                new Product()
                {
                    Name = "Dell XPS 13.3\" Laptop - Silver",
                    Producer = "Dell",
                    CategoryID = context.Categories.Single(c => c.Name == "Computers").ID,
                    UnitPrice = 1299.99M,
                    ImageUrl = "http://www.bestbuy.ca/multimedia/Products/150x150/103/10377/10377662.jpg"
                },
                new Product()
                {
                    Name = "Asus Zenbook UX305 13.3\" Ultrabook",
                    Producer = "Asus",
                    CategoryID = context.Categories.Single(c => c.Name == "Computers").ID,
                    UnitPrice = 849.99M,
                    ImageUrl = "http://www.bestbuy.ca/multimedia/Products/150x150/103/10380/10380387.jpg"
                },
                new Product()
                {
                    Name = "Samsung Galaxy Tab 4 10.1\" 16GB",
                    Producer = "Samsung",
                    CategoryID = context.Categories.Single(c => c.Name == "Tablets").ID,
                    UnitPrice = 269.99M,
                    ImageUrl = "http://www.bestbuy.ca/multimedia/Products/150x150/102/10292/10292506.jpg"
                },
                new Product()
                {
                    Name = "Acer Iconia Tab 8\" 16GB",
                    Producer = "Acer",
                    CategoryID = context.Categories.Single(c => c.Name == "Tablets").ID,
                    UnitPrice = 159.99M,
                    ImageUrl = "http://www.bestbuy.ca/multimedia/Products/150x150/103/10366/10366096.jpg"
                },
                new Product()
                {
                    Name = "Microsoft Surface Pro 3 12\" 128GB",
                    Producer = "Microsoft",
                    CategoryID = context.Categories.Single(c => c.Name == "Tablets").ID,
                    UnitPrice = 1369.99M,
                    ImageUrl = "http://www.bestbuy.ca/multimedia/Products/150x150/103/10379/10379984.jpg"
                },
                new Product()
                {
                    Name = "LG 55\" 1080p HD IPS LED Smart TV",
                    Producer = "LG",
                    CategoryID = context.Categories.Single(c => c.Name == "TV").ID,
                    UnitPrice = 899.99M,
                    ImageUrl = "http://www.bestbuy.ca/multimedia/Products/150x150/103/10375/10375638.jpg"
                },
                new Product()
                {
                    Name = "Toshiba 40\" 1080p HD 60Hz LED TV",
                    Producer = "Toshiba",
                    CategoryID = context.Categories.Single(c => c.Name == "TV").ID,
                    UnitPrice = 349.99M,
                    ImageUrl = "http://www.bestbuy.ca/multimedia/Products/150x150/103/10363/10363057.jpg"
                },
                new Product()
                {
                    Name = "Samsung 55\" 4K Ultra HD LED Tizen Smart OS TV",
                    Producer = "Samsung",
                    CategoryID = context.Categories.Single(c => c.Name == "TV").ID,
                    UnitPrice = 1474.98M,
                    ImageUrl = "http://www.bestbuy.ca/multimedia/Products/150x150/103/10363/10363473.jpg"
                },
                new Product()
                {
                    Name = "Sony Cyber-shot HX300 20.4MP",
                    Producer = "Sony",
                    CategoryID = context.Categories.Single(c => c.Name == "Cameras").ID,
                    UnitPrice = 319.99M,
                    ImageUrl = "http://www.bestbuy.ca/multimedia/Products/150x150/103/10366/10366084.jpg"
                },
                new Product()
                {
                    Name = "Canon PowerShot ELPH 160 20.0MP",
                    Producer = "Canon",
                    CategoryID = context.Categories.Single(c => c.Name == "Cameras").ID,
                    UnitPrice = 99.99M,
                    ImageUrl = "http://www.bestbuy.ca/multimedia/Products/150x150/103/10361/10361690.jpg"
                },
                new Product()
                {
                    Name = "Nikon COOLPIX S33 Waterproof/Shockproof 13.2MP",
                    Producer = "Nikon",
                    CategoryID = context.Categories.Single(c => c.Name == "Cameras").ID,
                    UnitPrice = 149.99M,
                    ImageUrl = "http://www.bestbuy.ca/multimedia/Products/150x150/B00/B0006/B0006079.jpg"
                },
                new Product()
                {
                    Name = "Canon PowerShot SX610 HS Wi-Fi 20.2MP",
                    Producer = "Canon",
                    CategoryID = context.Categories.Single(c => c.Name == "Cameras").ID,
                    UnitPrice = 229.99M,
                    ImageUrl = "http://www.bestbuy.ca/multimedia/Products/150x150/103/10360/10360322.jpg"
                },
                new Product()
                {
                    Name = "AT&T Corded Phone With Answering Machine (CL4940) - Black",
                    Producer = "AT&T",
                    CategoryID = context.Categories.Single(c => c.Name == "Phones").ID,
                    UnitPrice = 49.99M,
                    ImageUrl = "http://www.bestbuy.ca/multimedia/Products/150x150/101/10198/10198849.jpg"
                },
                new Product()
                {
                    Name = "Panasonic 3-Handest DECT 6.0 Cordless Phone with Answering Machine (KXTGC253B) - Black",
                    Producer = "Panasonic",
                    CategoryID = context.Categories.Single(c => c.Name == "Phones").ID,
                    UnitPrice =89.99M,
                    ImageUrl = "http://www.bestbuy.ca/multimedia/Products/150x150/103/10374/10374530.jpg"
                },
                new Product()
                {
                    Name = "Panasonic 2-Handset DECT 6.0 Corded/Cordless Phone with Answering Machine (KXTGF352M)",
                    Producer = "Panasonic",
                    CategoryID = context.Categories.Single(c => c.Name == "Phones").ID,
                    UnitPrice = 129.99M,
                    ImageUrl = "http://www.bestbuy.ca/multimedia/Products/150x150/103/10363/10363355.jpg"
                },
                new Product()
                {
                    Name = "Ooma HD2 Handset",
                    Producer = "Ooma",
                    CategoryID = context.Categories.Single(c => c.Name == "Phones").ID,
                    UnitPrice = 59.99M,
                    ImageUrl = "http://www.bestbuy.ca/multimedia/Products/150x150/102/10292/10292996.jpg"
                },
                new Product()
                {
                    Name = "2-Piece Cimarron Contemporary Bonded Leather Sofa & Loveseat Set - Black",
                    Producer = "",
                    CategoryID = context.Categories.Single(c => c.Name == "Living Room").ID,
                    UnitPrice = 2186.00M,
                    ImageUrl = "http://www.bestbuy.ca/multimedia/Products/150x150/B00/B0005/B0005578.jpg"
                },
                new Product()
                {
                    Name = "3-Piece Sofa, Loveseat & Chair Set - Charcoal Grey",
                    Producer = "",
                    CategoryID = context.Categories.Single(c => c.Name == "Living Room").ID,
                    UnitPrice = 4537.97M,
                    ImageUrl = "http://www.bestbuy.ca/multimedia/Products/150x150/B00/B0004/B0004633.jpg"
                },
                new Product()
                {
                    Name = "3-Piece Sofa, Loveseat & Chair Set - White",
                    Producer = "",
                    CategoryID = context.Categories.Single(c => c.Name == "Living Room").ID,
                    UnitPrice = 2146.99M,
                    ImageUrl = "http://www.bestbuy.ca/multimedia/Products/150x150/B00/B0004/B0004635.jpg"
                },
                new Product()
                {
                    Name = "3-Piece Sofa, Loveseat & Chair Set - Grey",
                    Producer = "",
                    CategoryID = context.Categories.Single(c => c.Name == "Living Room").ID,
                    UnitPrice = 3949.97M,
                    ImageUrl = "http://www.bestbuy.ca/multimedia/Products/150x150/B00/B0004/B0004636.jpg"
                },
                new Product()
                {
                    Name = "Step One Contemporary Bed - Queen - Chocolate Brown",
                    Producer = "",
                    CategoryID = context.Categories.Single(c => c.Name == "Bedroom").ID,
                    UnitPrice = 199.99M,
                    ImageUrl = "http://www.bestbuy.ca/multimedia/Products/150x150/101/10193/10193048.jpg"
                },
                new Product()
                {
                    Name = "South Shore Step One Contemporary Double Bed - Black",
                    Producer = "",
                    CategoryID = context.Categories.Single(c => c.Name == "Bedroom").ID,
                    UnitPrice = 139.98M,
                    ImageUrl = "http://www.bestbuy.ca/multimedia/Products/150x150/101/10193/10193038.jpg"
                },
                new Product()
                {
                    Name = "Amisco Queen Metal Platform Bed - Stratus/Cobrizo",
                    Producer = "",
                    CategoryID = context.Categories.Single(c => c.Name == "Bedroom").ID,
                    UnitPrice = 779.97M,
                    ImageUrl = "http://www.bestbuy.ca/multimedia/Products/150x150/B00/B0005/B0005382.jpg"
                },
                new Product()
                {
                    Name = "Step One Contemporary Bed - Queen - Chocolate",
                    Producer = "",
                    CategoryID = context.Categories.Single(c => c.Name == "Bedroom").ID,
                    UnitPrice = 230.98M,
                    ImageUrl = "http://www.bestbuy.ca/multimedia/Products/150x150/102/10278/10278111.jpg"
                },
                new Product()
                {
                    Name = "South Shore Logik Single Bunk Bed - Pure White",
                    Producer = "",
                    CategoryID = context.Categories.Single(c => c.Name == "Kids Room").ID,
                    UnitPrice = 927.83M,
                    ImageUrl = "http://www.bestbuy.ca/multimedia/Products/150x150/B00/B0002/B0002306.jpg"
                },
                new Product()
                {
                    Name = "South Shore Logik Single Bunk Bed - Chocolate",
                    Producer = "",
                    CategoryID = context.Categories.Single(c => c.Name == "Kids Room").ID,
                    UnitPrice = 1007.36M,
                    ImageUrl = "http://www.bestbuy.ca/multimedia/Products/150x150/B00/B0002/B0002308.jpg"
                },
                new Product()
                {
                    Name = "Broderick Contemporary Corner Desk - Clear Glass",
                    Producer = "",
                    CategoryID = context.Categories.Single(c => c.Name == "Office").ID,
                    UnitPrice = 198.99M,
                    ImageUrl = "http://www.bestbuy.ca/multimedia/Products/150x150/103/10322/10322308.jpg"
                },
                new Product()
                {
                    Name = "Bestar Hampton 1-Drawer L-Shaped Corner Workstation - Sandgranite/Charcoal",
                    Producer = "",
                    CategoryID = context.Categories.Single(c => c.Name == "Office").ID,
                    UnitPrice = 358.98M,
                    ImageUrl = "http://www.bestbuy.ca/multimedia/Products/150x150/103/10368/10368269.jpg"
                },
                new Product()
                {
                    Name = "2-Drawer Computer Desk - Cappuccino",
                    Producer = "",
                    CategoryID = context.Categories.Single(c => c.Name == "Office").ID,
                    UnitPrice = 399.99M,
                    ImageUrl = "http://www.bestbuy.ca/multimedia/Products/150x150/103/10359/10359198.jpg"
                },
                new Product()
                {
                    Name = "Elements Monterey Manager & Executive Chair - Black",
                    Producer = "",
                    CategoryID = context.Categories.Single(c => c.Name == "Office").ID,
                    UnitPrice = 199.99M,
                    ImageUrl = "http://www.bestbuy.ca/multimedia/Products/150x150/103/10322/10322300.jpg"
                },
                new Product()
                {
                    Name = "Montecito Manager & Executive Chair - Black",
                    Producer = "",
                    CategoryID = context.Categories.Single(c => c.Name == "Office").ID,
                    UnitPrice = 299.99M,
                    ImageUrl = "http://www.bestbuy.ca/multimedia/Products/150x150/103/10322/10322301.jpg"
                },
            };

            context.Products.AddRange(products);
            context.SaveChanges();
        }

        private void AddInventories(InventoryDb context, LocationDb locationDb)
        {
            Random rand = new Random();
            foreach (Product p in context.Products)
            {
                foreach (Warehouse w in locationDb.Warehouses)
                {
                    context.Inventories.Add(
                            new InventoryItem()
                            {
                                ProductID = p.ID,
                                WarehouseID = w.ID,
                                UnitsInStock = (short)rand.Next(1, 40)
                            }
                        );
                }
            }

            context.SaveChanges();
        }
    }
}