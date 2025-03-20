using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CleanBrilliantCompany.Interface;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Control
{
    public class InventoryControl
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IProduct _productService;

        public InventoryControl(IInventoryRepository inventoryRepository, IProduct productService)
        {
            _inventoryRepository = inventoryRepository ?? throw new ArgumentNullException(nameof(inventoryRepository));
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }

        public void CreateDashboard(string name, DateTime startDate, DateTime endDate, int validityDuration)
        {
            var newDashboard = new InventoryDashboardRDM(name, startDate, endDate, validityDuration)
            {
                Type = 2
            };
            _inventoryRepository.SaveDashboard(newDashboard);
        }

        public InventoryDashboardRDM FetchDashboard()
        {
            var dashboard = _inventoryRepository.GetLatestDashboard();
            if (dashboard == null)
            {
                throw new InvalidOperationException("No dashboard available. Please create a dashboard first.");
            }

            // Fetch stock levels from IProduct
            var stockLevels = _productService.GetProductStockLevels();

            // Get existing thresholds or set defaults
            var thresholds = new Dictionary<int, int>();
            foreach (var productId in stockLevels.Keys)
            {
                if (!thresholds.ContainsKey(productId))
                {
                    // Set a default threshold (e.g., 100) for new products
                    thresholds[productId] = 100;
                }
            }

            // Update the dashboard with stock levels and thresholds
            dashboard.UpdateDashboardData(stockLevels, thresholds);
            dashboard.UpdateReplenishmentStatus();

            // Persist the updated dashboard
            _inventoryRepository.SaveDashboard(dashboard);

            return dashboard;
        }

        public void UpdateDashboard(Dictionary<int, int> stockUpdates)
        {
            var dashboard = FetchDashboard();
            Dictionary<int, int> latestStockLevels = FetchLatestStockLevels();
            Dictionary<int, int> latestThresholds = FetchLatestThresholds();

            dashboard.UpdateDashboardData(latestStockLevels, latestThresholds);
            dashboard.UpdateReplenishmentStatus();

            _inventoryRepository.SaveDashboard(dashboard);
            Console.WriteLine("Dashboard updated with latest stock and threshold values.");
        }

        public string GenerateReport()
        {
            var dashboard = FetchDashboard();
            var lowStock = CheckLowStock();
            var overStock = CheckOverStock();

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
                report.AppendLine($"<li>Product {productId}: {dashboard.GetStockLevel(productId)} (below threshold {dashboard.GetThreshold(productId)})</li>");
            }
            report.AppendLine("</ul>");
            report.AppendLine("<h2>Over Stock Products</h2><ul>");
            foreach (var productId in overStock)
            {
                report.AppendLine($"<li>Product {productId}: {dashboard.GetStockLevel(productId)} (above threshold {dashboard.GetThreshold(productId)})</li>");
            }
            report.AppendLine("</ul>");

            Console.WriteLine("Report generated.");
            return report.ToString();
        }

        private Dictionary<int, int> FetchLatestStockLevels()
        {
            var dashboard = FetchDashboard();
            return new Dictionary<int, int>(dashboard.GetAllStockLevels());
        }

        private Dictionary<int, int> FetchLatestThresholds()
        {
            var dashboard = FetchDashboard();
            return new Dictionary<int, int>(dashboard.GetAllThresholds());
        }

        public List<int> CheckLowStock()
        {
            var dashboard = FetchDashboard();
            return dashboard.GetAllStockLevels()
                .Where(stock => stock.Value < dashboard.GetThreshold(stock.Key))
                .Select(stock => stock.Key)
                .ToList();
        }

        public List<int> CheckOverStock()
        {
            var dashboard = FetchDashboard();
            return dashboard.GetAllStockLevels()
                .Where(stock => stock.Value > dashboard.GetThreshold(stock.Key) * 1.5)
                .Select(stock => stock.Key)
                .ToList();
        }

        public List<int> GenerateReplenishmentActions()
        {
            return CheckLowStock();
        }

        public List<int> PrioritiseReplenishment()
        {
            var dashboard = FetchDashboard();
            return CheckLowStock()
                .OrderBy(productId => dashboard.GetStockLevel(productId))
                .ToList();
        }

        public string GenerateLowStockAlert()
        {
            var lowStockProducts = CheckLowStock();
            return lowStockProducts.Count > 0
                ? $"Low stock alert for products: {string.Join(", ", lowStockProducts)}"
                : "No low stock alerts.";
        }

        public string GenerateOverStockAlert()
        {
            var overStockProducts = CheckOverStock();
            return overStockProducts.Count > 0
                ? $"Overstock alert for products: {string.Join(", ", overStockProducts)}"
                : "No overstock alerts.";
        }

        // Chart generation methods
        public string GenerateStockLevelChartData()
        {
            try
            {
                var stockLevels = _productService.GetProductStockLevels();
                if (stockLevels == null || !stockLevels.Any())
                {
                    throw new InvalidOperationException("No stock level data available.");
                }

                // Fetch data from the dashboard
                var dashboard = FetchDashboard();
                var thresholds = dashboard.GetAllThresholds();

                var labels = stockLevels.Keys.Select(id => $"Product {id}").ToList();
                var stockData = stockLevels.Values.ToList();
                var thresholdData = stockLevels.Keys.Select(id => thresholds.ContainsKey(id) ? thresholds[id] : 0).ToList();

                var chartData = new
                {
                    labels = labels,
                    datasets = new[]
                    {
                new
                {
                    label = "Stock Levels",
                    data = stockData,
                    backgroundColor = "rgba(75, 192, 192, 0.2)",
                    borderColor = "rgba(75, 192, 192, 1)",
                    borderWidth = 1
                },
                new
                {
                    label = "Thresholds",
                    data = thresholdData,
                    backgroundColor = "rgba(255, 99, 132, 0.2)",
                    borderColor = "rgba(255, 99, 132, 1)",
                    borderWidth = 1
                }
            }
                };

                return System.Text.Json.JsonSerializer.Serialize(chartData);
            }
            catch (Exception ex)
            {
                var errorChartData = new
                {
                    labels = new string[] { },
                    datasets = new[]
                    {
                new
                {
                    label = "Stock Levels",
                    data = new int[] { },
                    backgroundColor = "rgba(75, 192, 192, 0.2)",
                    borderColor = "rgba(75, 192, 192, 1)",
                    borderWidth = 1
                }
            }
                };
                return System.Text.Json.JsonSerializer.Serialize(errorChartData);
            }
        }
    }
}