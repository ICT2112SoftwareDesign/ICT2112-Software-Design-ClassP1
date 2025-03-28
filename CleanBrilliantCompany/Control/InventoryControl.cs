using CleanBrilliantCompany.Data;
using CleanBrilliantCompany.DTO;
using CleanBrilliantCompany.Entities;
using CleanBrilliantCompany.Interface;
using CleanBrilliantCompany.Models;
using System.Text;

namespace CleanBrilliantCompany.Control
{
    public class InventoryControl
    {
        private readonly AppDbContext _context;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IProduct _productService;

        public InventoryControl(AppDbContext context, IInventoryRepository inventoryRepository, IProduct productService)
        {
            _inventoryRepository = inventoryRepository ?? throw new ArgumentNullException(nameof(inventoryRepository));
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void CreateDashboard(string name, int validityDuration)
        {
            var newDashboard = new InventoryDashboardRDM(name, validityDuration);

            // Get all products
            var allProducts = _productService.getAllProducts();

            // Extract productId and quantity
            var stockLevels = allProducts.ToDictionary(p => p.productId, p => p.quantity);

            var dbThresholds = _inventoryRepository.GetAllProductThresholds();
            var thresholds = allProducts.ToDictionary(
                p => p.productId,
                p => dbThresholds.ContainsKey(p.productId) ? dbThresholds[p.productId] : 100
            );



            // Update dashboard with current data
            newDashboard.UpdateStockThreshold(
                stockLevels,
                thresholds.ToDictionary(kvp => kvp.Key, kvp => kvp.Value ?? 100)
            );
            newDashboard.UpdateReplenishmentStatus();
            newDashboard.GenerateAlerts(); // Generate alerts after updating data

            // Save the dashboard with the updated data
            _inventoryRepository.SaveDashboard(newDashboard);
        }

        public InventoryDashboardRDM FetchDashboard()
        {
            var dashboard = _inventoryRepository.GetLatestDashboard();
            if (dashboard == null)
            {
                throw new InvalidOperationException("No dashboard available. Please create a dashboard first.");
            }

            return dashboard;
        }

        public string GenerateReport()
        {
            var dashboard = FetchDashboard();
            var lowStock = CheckLowStock();
            var overStock = CheckOverStock();
            var alertStreaks = GetWeeklyConsecutiveAlertCounts();

            var report = new StringBuilder();
            report.AppendLine($"<h1>Inventory Report - {dashboard.Name}</h1>");
            report.AppendLine($"<p>Period: {dashboard.RequestedStartDate} to {dashboard.RequestedEndDate}</p>");
            report.AppendLine("<h2>Stock Levels</h2><ul>");
            foreach (var stock in dashboard.GetAllStockLevels())
            {
                report.AppendLine($"<li>Product {stock.Key}: {stock.Value} units (Threshold: {dashboard.GetThreshold(stock.Key)})</li>");
            }
            report.AppendLine("</ul>");
            report.AppendLine("<h2>Low Stock Products</h2><ul>");
            foreach (var productId in lowStock)
            {
                report.AppendLine($"<li>Product {productId}: {dashboard.GetStockLevel(productId)}</li>");
            }
            report.AppendLine("</ul>");
            report.AppendLine("<h2>Over Stock Products</h2><ul>");
            foreach (var productId in overStock)
            {
                report.AppendLine($"<li>Product {productId}: {dashboard.GetStockLevel(productId)}</li>");
            }
            report.AppendLine("</ul>");
            report.AppendLine("<h2>Consecutive Weekly Alerts</h2><ul>");
            foreach (var productId in alertStreaks.Keys)
            {
                var streaks = alertStreaks[productId];
                if (streaks.LowStockWeeks > 0)
                {
                    report.AppendLine($"<li>Product {productId}: Low Stock for {streaks.LowStockWeeks} week(s) in a row</li>");
                }
                if (streaks.OverStockWeeks > 0)
                {
                    report.AppendLine($"<li>Product {productId}: Over Stock for {streaks.OverStockWeeks} week(s) in a row</li>");
                }
            }
            report.AppendLine("</ul>");

            Console.WriteLine("Report generated.");
            return report.ToString();
        }

        public List<int> CheckLowStock(string category = null)
        {
            var dashboard = FetchDashboard();
            var allStockLevels = dashboard.GetAllStockLevels();
            var thresholds = dashboard.GetAllThresholds();
            var products = _productService.getAllProducts();

            if (!string.IsNullOrEmpty(category))
                products = products.Where(p => p.productCategory == category).ToList();

            var productIds = products.Select(p => p.productId).ToHashSet();

            return products
                .Where(p =>
                    allStockLevels.ContainsKey(p.productId) &&
                    thresholds.ContainsKey(p.productId) &&
                    allStockLevels[p.productId] < (thresholds[p.productId] * 0.35)) // 35% of threshold
                .Select(p => p.productId)
                .ToList();

        }

        public List<int> CheckOverStock(string category = null)
        {
            var dashboard = FetchDashboard();
            var allStockLevels = dashboard.GetAllStockLevels();
            var thresholds = dashboard.GetAllThresholds();
            var products = _productService.getAllProducts();

            if (!string.IsNullOrEmpty(category))
                products = products.Where(p => p.productCategory == category).ToList();

            var productIds = products.Select(p => p.productId).ToHashSet();

            return products
                .Where(p =>
                    allStockLevels.ContainsKey(p.productId) &&
                    thresholds.ContainsKey(p.productId) &&
                    allStockLevels[p.productId] > (thresholds[p.productId] * 1.6)) // 160% of threshold
                .Select(p => p.productId)
                .ToList();
        }


        public List<int> GenerateReplenishmentActions(string category = null)
        {
            return CheckLowStock(category);
        }

        public string GenerateStockLevelChartData(string category = null)
        {
            var dashboard = FetchDashboard();
            var stockLevels = dashboard.GetAllStockLevels();
            var thresholds = _context.ProductThresholdTable
                .ToDictionary(t => t.ProductId, t => t.Threshold ?? 100);
            var productIds = stockLevels.Keys.ToList();

            var products = _productService.getAllProducts()
                .Where(p => productIds.Contains(p.productId))
                .ToList();

            if (!string.IsNullOrEmpty(category))
            {
                products = products.Where(p => p.productCategory == category).ToList();
            }

            var chartData = new
            {
                labels = products.Select(p => p.productId).ToList(),
                datasets = new[] {
                    new {
                        label = "Stock Levels",
                        data = products.Select(p => stockLevels[p.productId]).ToList()
                    },
                    new {
                        label = "Thresholds",
                        data = products.Select(p => thresholds[p.productId]).ToList()
                    }
                },
                products = products.Select(p => new
                {
                    id = p.productId,
                    name = p.productName,
                    category = p.productCategory,
                    stock = stockLevels[p.productId],
                    threshold = thresholds[p.productId]
                }).ToList()
            };

            return System.Text.Json.JsonSerializer.Serialize(chartData);
        }

        public Dictionary<int, (int LowStockWeeks, int OverStockWeeks)> GetWeeklyConsecutiveAlertCounts()
        {
            return _inventoryRepository.GetConsecutiveWeeklyAlerts();
        }

        public Dictionary<int, InventoryDTO> GetProductLookup()
        {
            var dashboard = FetchDashboard();
            var stockLevels = dashboard.GetAllStockLevels();
            var thresholds = dashboard.GetAllThresholds();
            var replenishmentStatuses = dashboard.GetAllReplenishmentStatuses();

            return _productService.getAllProducts()
                .ToDictionary(p => p.productId, p => new InventoryDTO
                {
                    ProductId = p.productId,
                    ProductName = p.productName,
                    ProductCategory = p.productCategory,
                    StockLevel = stockLevels.ContainsKey(p.productId) ? stockLevels[p.productId] : 0,
                    Threshold = thresholds.ContainsKey(p.productId) ? thresholds[p.productId] : 100,
                    ReplenishmentStatus = replenishmentStatuses.ContainsKey(p.productId) && replenishmentStatuses[p.productId],
                });
        }

        public List<ProductTable> GetAllProducts()
        {
            return _productService.getAllProducts();
        }

        public InventoryDTO GetThresholdById(int id)
        {
            var product = _productService.getAllProducts().FirstOrDefault(p => p.productId == id);
            var threshold = _context.ProductThresholdTable.FirstOrDefault(t => t.ProductId == id);

            return new InventoryDTO
            {
                ProductId = id,
                ProductName = product?.productName ?? "Unknown",
                ProductCategory = product?.productCategory ?? "Unknown",
                Threshold = threshold?.Threshold ?? 100,
                LastUpdated = threshold?.LastUpdated
            };
        }



        public void UpdateThreshold(ProductThresholdTable model)
        {
            var entry = _context.ProductThresholdTable.FirstOrDefault(t => t.ProductId == model.ProductId);
            if (entry != null)
            {
                entry.Threshold = model.Threshold;
                entry.LastUpdated = DateTime.Now;
            }
            else
            {
                _context.ProductThresholdTable.Add(model);
            }

            _context.SaveChanges();
        }

        public List<InventoryDTO> GetProductThresholdsWithInfo()
        {
            return _context.ProductThresholdTable
                .Join(
                    _context.ProductTable,
                    pt => pt.ProductId,
                    p => p.productId,
                    (pt, p) => new InventoryDTO
                    {
                        ProductId = p.productId,
                        ProductName = p.productName,
                        Threshold = pt.Threshold ?? 100, // Handle NULL if needed
                        LastUpdated = pt.LastUpdated,
                    })
                .ToList();
        }

        public void InitializeMissingThresholds()
        {
            // Get all product IDs
            var allProductIds = _productService.getAllProducts()
                .Select(p => p.productId)
                .ToList();

            // Get product IDs that already have thresholds
            var existingThresholdProductIds = _context.ProductThresholdTable
                .Select(pt => pt.ProductId)
                .ToList();

            // Find missing product IDs
            var missingProductIds = allProductIds
                .Except(existingThresholdProductIds)
                .ToList();

            // Insert default thresholds (100) for missing products
            foreach (var productId in missingProductIds)
            {
                _context.ProductThresholdTable.Add(new ProductThresholdTable
                {
                    ProductId = productId,
                    Threshold = 100,
                    LastUpdated = DateTime.Now
                });
            }

            _context.SaveChanges();
        }
    }
}