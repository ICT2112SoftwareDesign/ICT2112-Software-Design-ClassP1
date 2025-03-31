using CleanBrilliantCompany.DTO;  // Ensure DTO namespace is included

public class CostSimulation
{
    public List<DashboardDTO> Dashboards { get; set; }
    public List<ProductManufacturerDTO> Manufacturers { get; set; }
    public List<ProductBatchDTO> ProductBatches { get; set; }
    public List<ItemDTO> Items { get; set; }

    public CostSimulation()
    {
        // // 🔹 Fake Dashboards
        // Dashboards = new List<DashboardDTO>
        // {
        //     new DashboardDTO
        //     {
        //         DashboardId = 1,
        //         Name = "Cleaning Supplies Cost Dashboard",
        //         RequestedStartDate = DateTime.Now.AddMonths(-6),
        //         RequestedEndDate = DateTime.Now,
        //         GeneratedDate = DateTime.Now.AddDays(-1),
        //         ValidityDuration = 5,
        //         TypeId = 1
        //     }
        // };
        

        // 🔹 Fake Cleaning & Laundry Product Manufacturers
        Manufacturers = new List<ProductManufacturerDTO>
        {
            new ProductManufacturerDTO { ManufacturerId = 1, CompanyName = "Procter & Gamble", Address = "1 Procter & Gamble Plaza, Cincinnati, OH, USA", Email = "contact@pg.com" },
            new ProductManufacturerDTO { ManufacturerId = 2, CompanyName = "Unilever", Address = "Unilever House, 100 Victoria Embankment, London, UK", Email = "info@unilever.com" },
            new ProductManufacturerDTO { ManufacturerId = 3, CompanyName = "Colgate-Palmolive", Address = "300 Park Ave, New York, NY, USA", Email = "contact@colpal.com" },
            new ProductManufacturerDTO { ManufacturerId = 4, CompanyName = "Henkel", Address = "Henkelstr. 67, 40589 Düsseldorf, Germany", Email = "info@henkel.com" },
            new ProductManufacturerDTO { ManufacturerId = 5, CompanyName = "Reckitt Benckiser", Address = "103-105 Bath Road, Slough, UK", Email = "support@reckitt.com" },
            new ProductManufacturerDTO { ManufacturerId = 6, CompanyName = "Clorox Company", Address = "1221 Broadway, Oakland, CA, USA", Email = "info@clorox.com" }
        };

        // 🔹 Fake Cleaning & Laundry Product Batches
        ProductBatches = new List<ProductBatchDTO>
        {
            // Procter & Gamble (Laundry Detergents & Cleaning Supplies)
            new ProductBatchDTO { BatchCode = 201, ProductId = 601, BatchPrice = 750m, BatchQuantity = 500, ExpiryDate = new DateTime(2026, 06, 15), ReceiveDate = new DateTime(2025, 02, 10), ManufactureDate = new DateTime(2024, 10, 01), ManufacturerId = 1 }, // Tide Detergent
            new ProductBatchDTO { BatchCode = 202, ProductId = 602, BatchPrice = 500m, BatchQuantity = 600, ExpiryDate = new DateTime(2026, 07, 20), ReceiveDate = new DateTime(2025, 03, 12), ManufactureDate = new DateTime(2024, 11, 10), ManufacturerId = 1 }, // Mr. Clean All-Purpose Cleaner

            // Unilever (Laundry & Dishwashing Products)
            new ProductBatchDTO { BatchCode = 203, ProductId = 603, BatchPrice = 600m, BatchQuantity = 700, ExpiryDate = new DateTime(2026, 08, 10), ReceiveDate = new DateTime(2025, 04, 15), ManufactureDate = new DateTime(2024, 12, 05), ManufacturerId = 2 }, // OMO Laundry Powder
            new ProductBatchDTO { BatchCode = 204, ProductId = 604, BatchPrice = 400m, BatchQuantity = 500, ExpiryDate = new DateTime(2026, 09, 30), ReceiveDate = new DateTime(2025, 05, 20), ManufactureDate = new DateTime(2025, 01, 15), ManufacturerId = 2 }, // Sunlight Dish Soap

            // Colgate-Palmolive (Personal & Home Cleaning)
            new ProductBatchDTO { BatchCode = 205, ProductId = 605, BatchPrice = 550m, BatchQuantity = 800, ExpiryDate = new DateTime(2027, 01, 25), ReceiveDate = new DateTime(2025, 06, 05), ManufactureDate = new DateTime(2025, 02, 01), ManufacturerId = 3 }, // Palmolive Dishwashing Liquid
            new ProductBatchDTO { BatchCode = 206, ProductId = 606, BatchPrice = 700m, BatchQuantity = 900, ExpiryDate = new DateTime(2027, 02, 28), ReceiveDate = new DateTime(2025, 07, 10), ManufactureDate = new DateTime(2025, 03, 10), ManufacturerId = 3 }, // Ajax Multi-Purpose Cleaner

            // Henkel (Laundry & Surface Cleaning)
            new ProductBatchDTO { BatchCode = 207, ProductId = 607, BatchPrice = 850m, BatchQuantity = 950, ExpiryDate = new DateTime(2027, 05, 15), ReceiveDate = new DateTime(2025, 08, 20), ManufactureDate = new DateTime(2025, 04, 15), ManufacturerId = 4 }, // Persil Laundry Liquid
            new ProductBatchDTO { BatchCode = 208, ProductId = 608, BatchPrice = 650m, BatchQuantity = 700, ExpiryDate = new DateTime(2027, 07, 10), ReceiveDate = new DateTime(2025, 09, 18), ManufactureDate = new DateTime(2025, 05, 20), ManufacturerId = 4 }, // Bref Bathroom Cleaner

            // Reckitt Benckiser (Disinfectants & Air Fresheners)
            new ProductBatchDTO { BatchCode = 209, ProductId = 609, BatchPrice = 950m, BatchQuantity = 750, ExpiryDate = new DateTime(2027, 10, 05), ReceiveDate = new DateTime(2025, 10, 15), ManufactureDate = new DateTime(2025, 06, 10), ManufacturerId = 5 }, // Lysol Disinfectant Spray
            new ProductBatchDTO { BatchCode = 210, ProductId = 610, BatchPrice = 1100m, BatchQuantity = 800, ExpiryDate = new DateTime(2028, 01, 10), ReceiveDate = new DateTime(2025, 11, 20), ManufactureDate = new DateTime(2025, 07, 05), ManufacturerId = 5 }, // Air Wick Freshener

            // Clorox (Bleach & Cleaning Wipes)
            new ProductBatchDTO { BatchCode = 211, ProductId = 611, BatchPrice = 700m, BatchQuantity = 850, ExpiryDate = new DateTime(2028, 03, 15), ReceiveDate = new DateTime(2025, 12, 05), ManufactureDate = new DateTime(2025, 08, 01), ManufacturerId = 6 }, // Clorox Bleach
            new ProductBatchDTO { BatchCode = 212, ProductId = 612, BatchPrice = 800m, BatchQuantity = 900, ExpiryDate = new DateTime(2028, 06, 10), ReceiveDate = new DateTime(2026, 01, 10), ManufactureDate = new DateTime(2025, 09, 10), ManufacturerId = 6 },  // Clorox Disinfecting Wipes Lemon
            new ProductBatchDTO { BatchCode = 213, ProductId = 613, BatchPrice = 850m, BatchQuantity = 850, ExpiryDate = new DateTime(2028, 03, 15), ReceiveDate = new DateTime(2025, 12, 05), ManufactureDate = new DateTime(2025, 08, 01), ManufacturerId = 6 }, // Clorox Bleach 
            new ProductBatchDTO { BatchCode = 214, ProductId = 611, BatchPrice = 720m, BatchQuantity = 870, ExpiryDate = new DateTime(2028, 04, 12), ReceiveDate = new DateTime(2025, 11, 20), ManufactureDate = new DateTime(2025, 07, 18), ManufacturerId = 6 },
            new ProductBatchDTO { BatchCode = 215, ProductId = 612, BatchPrice = 830m, BatchQuantity = 920, ExpiryDate = new DateTime(2028, 07, 08), ReceiveDate = new DateTime(2026, 02, 05), ManufactureDate = new DateTime(2025, 10, 14), ManufacturerId = 6 },
            new ProductBatchDTO { BatchCode = 216, ProductId = 613, BatchPrice = 860m, BatchQuantity = 880, ExpiryDate = new DateTime(2028, 02, 28), ReceiveDate = new DateTime(2025, 12, 22), ManufactureDate = new DateTime(2025, 08, 11), ManufacturerId = 6 },
            new ProductBatchDTO { BatchCode = 217, ProductId = 614, BatchPrice = 610m, BatchQuantity = 910, ExpiryDate = new DateTime(2028, 05, 20), ReceiveDate = new DateTime(2026, 01, 18), ManufactureDate = new DateTime(2025, 09, 05), ManufacturerId = 6 },
            new ProductBatchDTO { BatchCode = 218, ProductId = 614, BatchPrice = 560m, BatchQuantity = 1030, ExpiryDate = new DateTime(2028, 08, 30), ReceiveDate = new DateTime(2026, 03, 15), ManufactureDate = new DateTime(2025, 10, 22), ManufacturerId = 6 },
            new ProductBatchDTO { BatchCode = 219, ProductId = 614, BatchPrice = 790m, BatchQuantity = 1180, ExpiryDate = new DateTime(2028, 07, 25), ReceiveDate = new DateTime(2026, 02, 10), ManufactureDate = new DateTime(2025, 09, 01), ManufacturerId = 6 },
            new ProductBatchDTO { BatchCode = 220, ProductId = 614, BatchPrice = 620m, BatchQuantity = 940, ExpiryDate = new DateTime(2028, 06, 18), ReceiveDate = new DateTime(2026, 04, 12), ManufactureDate = new DateTime(2025, 10, 09), ManufacturerId = 6 },
            new ProductBatchDTO { BatchCode = 221, ProductId = 614, BatchPrice = 640m, BatchQuantity = 980, ExpiryDate = new DateTime(2028, 09, 15), ReceiveDate = new DateTime(2026, 05, 17), ManufactureDate = new DateTime(2025, 11, 03), ManufacturerId = 6 },
            new ProductBatchDTO { BatchCode = 222, ProductId = 614, BatchPrice = 700m, BatchQuantity = 6, ExpiryDate = new DateTime(2028, 06, 10), ReceiveDate = new DateTime(2026, 06, 10), ManufactureDate = new DateTime(2025, 09, 10), ManufacturerId = 6 }  
        };

                // Sample entries for ProductBatchItemDTO
        Items = new List<ItemDTO>
        {
            // Entries with BatchCode 219 and Sold status
            new ItemDTO { ItemId = 5001, ProductId = 614, BatchCode = 222, SalePrice = 100m, WarehouseId = 3, ItemStatus = "Sold", OrderId = 1001 },
            new ItemDTO { ItemId = 5002, ProductId = 614, BatchCode = 222, SalePrice = 100m, WarehouseId = 3, ItemStatus = "Sold", OrderId = 1002 },
            new ItemDTO { ItemId = 5003, ProductId = 614, BatchCode = 222, SalePrice = 100m, WarehouseId = 4, ItemStatus = "Sold", OrderId = 1003 },
            new ItemDTO { ItemId = 5004, ProductId = 614, BatchCode = 222, SalePrice = 100m, WarehouseId = 1, ItemStatus = "Sold", OrderId = 1004 },
            new ItemDTO { ItemId = 5005, ProductId = 614, BatchCode = 222, SalePrice = 100m, WarehouseId = 2, ItemStatus = "Sold", OrderId = 1005 },
            new ItemDTO { ItemId = 5006, ProductId = 614, BatchCode = 222, SalePrice = 60m, WarehouseId = 2, ItemStatus = "Sold", OrderId = 1006 },
            new ItemDTO { ItemId = 5007, ProductId = 611, BatchCode = 211, SalePrice = 50m, WarehouseId = 2, ItemStatus = "Sold", OrderId = 1007 },
        };


    }
}