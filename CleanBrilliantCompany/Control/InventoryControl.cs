using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CleanBrilliantCompany.DTO;
using CleanBrilliantCompany.Entities;
using CleanBrilliantCompany.Interface;
using CleanBrilliantCompany.Models;
using Microsoft.EntityFrameworkCore.Infrastructure;

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

        public void CreateDashboard(string name, int validityDuration)
        {
            var newDashboard = new InventoryDashboardRDM(name, validityDuration);

            // Fetch stock levels from IProduct
            // var stockLevels = _productService.GetProductStockLevels();

            // Get all products
            var allProducts = _productService.getAllProducts();
            // Extract productId and quantity
            var stockLevels = allProducts.ToDictionary(p => p.productId, p => p.quantity);

            // Set default thresholds (e.g., 100) — you can customize this logic
            var thresholds = allProducts.ToDictionary(p => p.productId, p => 100);



            // Update dashboard with stock levels and thresholds
            newDashboard.UpdateDashboardData(stockLevels, thresholds);
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
            //var alertCounts = _inventoryRepository.GetAlertCounts();
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
                report.AppendLine($"<li>Product {productId}: {dashboard.GetStockLevel(productId)} (below threshold {dashboard.GetThreshold(productId)})</li>");
            }
            report.AppendLine("</ul>");
            report.AppendLine("<h2>Over Stock Products</h2><ul>");
            foreach (var productId in overStock)
            {
                report.AppendLine($"<li>Product {productId}: {dashboard.GetStockLevel(productId)} (above threshold {dashboard.GetThreshold(productId)})</li>");
            }
            report.AppendLine("</ul>");
            report.AppendLine("<h2>Alerts</h2>");
            //report.AppendLine($"<p>{dashboard.LowStockAlert}</p>");
            //report.AppendLine($"<p>{dashboard.OverStockAlert}</p>");
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

            return allStockLevels
                .Where(kvp => productIds.Contains(kvp.Key) && kvp.Value < thresholds[kvp.Key] * 0.35)
                .Select(kvp => kvp.Key)
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

            return allStockLevels
                .Where(kvp => productIds.Contains(kvp.Key) && kvp.Value > thresholds[kvp.Key] * 1.6)
                .Select(kvp => kvp.Key)
                .ToList();
        }

        //public List<int> GenerateReplenishmentActions()
        //{
        //    return CheckLowStock();
        //}
        public List<int> GenerateReplenishmentActions(string category = null)
        {
            return CheckLowStock(category);
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

        public string GenerateStockLevelChartData(string category = null)
        {
            var dashboard = FetchDashboard();
            var allStockLevels = dashboard.GetAllStockLevels();
            var allThresholds = dashboard.GetAllThresholds();

            var products = _productService.getAllProducts();

            if (!string.IsNullOrEmpty(category))
            {
                products = products.Where(p => p.productCategory == category).ToList();
            }

            var productIds = products.Select(p => p.productId).ToList();

            var stockLevels = allStockLevels.Where(kvp => productIds.Contains(kvp.Key)).ToDictionary(k => k.Key, v => v.Value);
            var thresholds = allThresholds.Where(kvp => productIds.Contains(kvp.Key)).ToDictionary(k => k.Key, v => v.Value);

            var labels = productIds.Select(id => $"Product {id}").ToList();
            var stockData = productIds.Select(id => stockLevels.ContainsKey(id) ? stockLevels[id] : 0).ToList();
            var thresholdData = productIds.Select(id => thresholds.ContainsKey(id) ? thresholds[id] : 0).ToList();

            var chartData = new
            {
                labels = productIds.Select(id => $"Product {id}").ToList(),
                products = productIds.Select(id =>
                {
                    var p = products.First(prod => prod.productId == id);
                    return new
                    {
                        id = p.productId,
                        name = p.productName,
                        category = p.productCategory,
                        stock = stockLevels.ContainsKey(p.productId) ? stockLevels[p.productId] : 0
                    };
                }).ToList(),
                datasets = new[]
                {
        new
        {
            label = "Stock Levels",
            data = stockData
        },
        new
        {
            label = "Thresholds",
            data = thresholdData
        }
    }
            };

            return System.Text.Json.JsonSerializer.Serialize(chartData);
        }




        public Dictionary<int, (int LowStockWeeks, int OverStockWeeks)> GetWeeklyConsecutiveAlertCounts()
        {
            return _inventoryRepository.GetConsecutiveWeeklyAlerts();
        }

        public Dictionary<int, InventoryDTO> GetProductLookup()
        {
            var stockLevels = FetchDashboard().GetAllStockLevels();
            return _productService.getAllProducts()
                .ToDictionary(p => p.productId, p => new InventoryDTO
                {
                    ProductId = p.productId,
                    ProductName = p.productName,
                    ProductCategory = p.productCategory,
                    StockLevel = stockLevels.ContainsKey(p.productId) ? stockLevels[p.productId] : 0
                });
        }

        public List<ProductTable> GetAllProducts()
        {
            return _productService.getAllProducts();
        }
    }
}