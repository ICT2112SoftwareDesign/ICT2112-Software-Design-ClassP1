// using CleanBrilliantCompany.DTO;  // Ensure DTO namespace is included

// public class CostSimulation
// {
//     public List<DashboardDTO> Dashboards { get; set; }
//     public List<ProductManufacturerDTO> Manufacturers { get; set; }
//     public List<ProductBatchDTO> ProductBatches { get; set; }

//     public CostSimulation()
//     {
//         // 🔹 Fake Dashboards
//         Dashboards = new List<DashboardDTO>
//         {
//             new DashboardDTO
//             {
//                 DashboardId = 1,
//                 Name = "Cost Dashboard 1",
//                 RequestedStartDate = DateTime.Now.AddMonths(-6),
//                 RequestedEndDate = DateTime.Now,
//                 GeneratedDate = DateTime.Now.AddDays(-1),
//                 ValidityDuration = 5,
//                 Type = 1
//             }
//         };

//         // 🔹 Fake Product Manufacturers
//         Manufacturers = new List<ProductManufacturerDTO>
//         {
//             new ProductManufacturerDTO { ManufacturerId = 1, CompanyName = "Mars, Incorporated", Address = "6885 Elm St, McLean, VA 22101, USA", Email = "contact@mars.com" },
//             new ProductManufacturerDTO { ManufacturerId = 2, CompanyName = "Ferrero Group", Address = "Piazzale Pietro Ferrero 1, 12051 Alba (CN), Italy", Email = "info@ferrero.com" },
//             new ProductManufacturerDTO { ManufacturerId = 3, CompanyName = "Mondelez International", Address = "905 West Fulton Market, Suite 200, Chicago, IL 60607, USA", Email = "contactus@mdlz.com" },
//             new ProductManufacturerDTO { ManufacturerId = 4, CompanyName = "Nestlé S.A.", Address = "Avenue Nestlé 55, 1800 Vevey, Switzerland", Email = "info@nestle.com" },
//             new ProductManufacturerDTO { ManufacturerId = 5, CompanyName = "Hershey Company", Address = "19 E Chocolate Ave, Hershey, PA 17033, USA", Email = "contact@hersheys.com" },
//             new ProductManufacturerDTO { ManufacturerId = 6, CompanyName = "Lindt & Sprüngli", Address = "Seestrasse 204, 8802 Kilchberg, Switzerland", Email = "info@lindt.com" },
//             new ProductManufacturerDTO { ManufacturerId = 7, CompanyName = "Godiva Chocolatier", Address = "333 W 34th St, New York, NY 10001, USA", Email = "info@godiva.com" },
//             new ProductManufacturerDTO { ManufacturerId = 8, CompanyName = "Barry Callebaut", Address = "Mühlenstrasse 8, 8570 Weinfelden, Switzerland", Email = "contact@barry-callebaut.com" },
//             new ProductManufacturerDTO { ManufacturerId = 9, CompanyName = "Toblerone", Address = "Bärengasse 29, 8001 Zürich, Switzerland", Email = "info@toblerone.com" },
//             new ProductManufacturerDTO { ManufacturerId = 10, CompanyName = "Cadbury", Address = "Bournville, Birmingham, B30 2LU, UK", Email = "contact@cadbury.co.uk" }
//         };

//         // 🔹 Fake Product Batches
//         ProductBatches = new List<ProductBatchDTO>
//         {
//             new ProductBatchDTO
//             {
//                 BatchCode = 101,
//                 ProductId = 501,
//                 BatchPrice = 500m,
//                 BatchQuantity = 1000,
//                 ExpiryDate = new DateTime(2026, 03, 01),
//                 ReceiveDate = new DateTime(2025, 03, 10),
//                 ManufactureDate = new DateTime(2024, 09, 01),
//                 ManufacturerId = 1 // Mars, Incorporated
//             },
//             new ProductBatchDTO
//             {
//                 BatchCode = 102,
//                 ProductId = 502,
//                 BatchPrice = 800m,
//                 BatchQuantity = 1000,
//                 ExpiryDate = new DateTime(2026, 04, 15),
//                 ReceiveDate = new DateTime(2025, 03, 12),
//                 ManufactureDate = new DateTime(2024, 10, 15),
//                 ManufacturerId = 2 // Ferrero Group
//             },
//             new ProductBatchDTO
//             {
//                 BatchCode = 103,
//                 ProductId = 501,
//                 BatchPrice = 400m,
//                 BatchQuantity = 1000,
//                 ExpiryDate = new DateTime(2026, 05, 20),
//                 ReceiveDate = new DateTime(2025, 03, 15),
//                 ManufactureDate = new DateTime(2024, 11, 20),
//                 ManufacturerId = 1 // Mars, Incorporated
//             },
//             new ProductBatchDTO
//             {
//                 BatchCode = 104,
//                 ProductId = 503,
//                 BatchPrice = 1300m,
//                 BatchQuantity = 1000,
//                 ExpiryDate = new DateTime(2026, 06, 25),
//                 ReceiveDate = new DateTime(2025, 03, 18),
//                 ManufactureDate = new DateTime(2024, 12, 25),
//                 ManufacturerId = 3 // Mondelez International
//             },
//             new ProductBatchDTO
//             {
//                 BatchCode = 105,
//                 ProductId = 502,
//                 BatchPrice = 800m,
//                 BatchQuantity = 1000,
//                 ExpiryDate = new DateTime(2026, 07, 30),
//                 ReceiveDate = new DateTime(2025, 03, 20),
//                 ManufactureDate = new DateTime(2025, 01, 30),
//                 ManufacturerId = 2 // Ferrero Group
//             },
//             new ProductBatchDTO
//             {
//                 BatchCode = 106,
//                 ProductId = 504,
//                 BatchPrice = 950m,
//                 BatchQuantity = 1200,
//                 ExpiryDate = new DateTime(2026, 08, 15),
//                 ReceiveDate = new DateTime(2025, 03, 25),
//                 ManufactureDate = new DateTime(2025, 02, 15),
//                 ManufacturerId = 4 // Nestlé S.A.
//             },
//             new ProductBatchDTO
//             {
//                 BatchCode = 107,
//                 ProductId = 505,
//                 BatchPrice = 1100m,
//                 BatchQuantity = 900,
//                 ExpiryDate = new DateTime(2026, 09, 10),
//                 ReceiveDate = new DateTime(2025, 04, 01),
//                 ManufactureDate = new DateTime(2025, 03, 01),
//                 ManufacturerId = 5 // Hershey Company
//             },
//             new ProductBatchDTO
//             {
//                 BatchCode = 108,
//                 ProductId = 506,
//                 BatchPrice = 1200m,
//                 BatchQuantity = 1500,
//                 ExpiryDate = new DateTime(2026, 10, 05),
//                 ReceiveDate = new DateTime(2025, 04, 08),
//                 ManufactureDate = new DateTime(2025, 03, 15),
//                 ManufacturerId = 6 // Lindt & Sprüngli
//             },
//             new ProductBatchDTO
//             {
//                 BatchCode = 109,
//                 ProductId = 507,
//                 BatchPrice = 1050m,
//                 BatchQuantity = 1100,
//                 ExpiryDate = new DateTime(2026, 11, 20),
//                 ReceiveDate = new DateTime(2025, 04, 15),
//                 ManufactureDate = new DateTime(2025, 03, 20),
//                 ManufacturerId = 7 // Godiva Chocolatier
//             },
//             new ProductBatchDTO
//             {
//                 BatchCode = 110,
//                 ProductId = 508,
//                 BatchPrice = 1300m,
//                 BatchQuantity = 1300,
//                 ExpiryDate = new DateTime(2026, 12, 30),
//                 ReceiveDate = new DateTime(2025, 04, 22),
//                 ManufactureDate = new DateTime(2025, 04, 01),
//                 ManufacturerId = 8 // Barry Callebaut
//             },
//             new ProductBatchDTO
//             {
//                 BatchCode = 111,
//                 ProductId = 509,
//                 BatchPrice = 750m,
//                 BatchQuantity = 800,
//                 ExpiryDate = new DateTime(2027, 01, 15),
//                 ReceiveDate = new DateTime(2025, 04, 30),
//                 ManufactureDate = new DateTime(2025, 04, 10),
//                 ManufacturerId = 9 // Toblerone
//             },
//             new ProductBatchDTO
//             {
//                 BatchCode = 112,
//                 ProductId = 510,
//                 BatchPrice = 1400m,
//                 BatchQuantity = 700,
//                 ExpiryDate = new DateTime(2027, 02, 10),
//                 ReceiveDate = new DateTime(2025, 05, 05),
//                 ManufactureDate = new DateTime(2025, 04, 20),
//                 ManufacturerId = 10 // Cadbury
//             },
//             new ProductBatchDTO
//             {
//                 BatchCode = 113,
//                 ProductId = 501,
//                 BatchPrice = 500m,
//                 BatchQuantity = 1000,
//                 ExpiryDate = new DateTime(2027, 03, 01),
//                 ReceiveDate = new DateTime(2025, 05, 12),
//                 ManufactureDate = new DateTime(2025, 05, 01),
//                 ManufacturerId = 1 // Mars, Incorporated
//             },
//             new ProductBatchDTO
//             {
//                 BatchCode = 114,
//                 ProductId = 502,
//                 BatchPrice = 800m,
//                 BatchQuantity = 1000,
//                 ExpiryDate = new DateTime(2027, 04, 15),
//                 ReceiveDate = new DateTime(2025, 05, 18),
//                 ManufactureDate = new DateTime(2025, 05, 10),
//                 ManufacturerId = 2 // Ferrero Group
//             },
//             new ProductBatchDTO
//             {
//                 BatchCode = 115,
//                 ProductId = 503,
//                 BatchPrice = 1300m,
//                 BatchQuantity = 1000,
//                 ExpiryDate = new DateTime(2027, 06, 25),
//                 ReceiveDate = new DateTime(2025, 06, 01),
//                 ManufactureDate = new DateTime(2025, 05, 15),
//                 ManufacturerId = 3 // Mondelez International
//             },
//             new ProductBatchDTO
//             {
//                 BatchCode = 116,
//                 ProductId = 504,
//                 BatchPrice = 950m,
//                 BatchQuantity = 1200,
//                 ExpiryDate = new DateTime(2027, 08, 15),
//                 ReceiveDate = new DateTime(2025, 06, 10),
//                 ManufactureDate = new DateTime(2025, 06, 01),
//                 ManufacturerId = 4 // Nestlé S.A.
//             },
//             new ProductBatchDTO
//             {
//                 BatchCode = 117,
//                 ProductId = 505,
//                 BatchPrice = 1100m,
//                 BatchQuantity = 900,
//                 ExpiryDate = new DateTime(2027, 09, 10),
//                 ReceiveDate = new DateTime(2025, 06, 18),
//                 ManufactureDate = new DateTime(2025, 06, 05),
//                 ManufacturerId = 5 // Hershey Company
//             },
//             new ProductBatchDTO
//             {
//                 BatchCode = 118,
//                 ProductId = 506,
//                 BatchPrice = 1200m,
//                 BatchQuantity = 1500,
//                 ExpiryDate = new DateTime(2027, 10, 05),
//                 ReceiveDate = new DateTime(2025, 06, 25),
//                 ManufactureDate = new DateTime(2025, 06, 10),
//                 ManufacturerId = 6 // Lindt & Sprüngli
//             },
//             new ProductBatchDTO
//             {
//                 BatchCode = 119,
//                 ProductId = 507,
//                 BatchPrice = 1050m,
//                 BatchQuantity = 1100,
//                 ExpiryDate = new DateTime(2027, 11, 20),
//                 ReceiveDate = new DateTime(2025, 07, 01),
//                 ManufactureDate = new DateTime(2025, 06, 15),
//                 ManufacturerId = 7 // Godiva Chocolatier
//             }
//         };
//     }
// }
