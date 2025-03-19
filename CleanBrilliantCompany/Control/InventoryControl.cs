using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Interface;

namespace CleanBrilliantCompany.Control
{
    public class InventoryControl
    {
        private readonly IInventoryRepository _inventoryRepository;

        public InventoryControl(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository ?? throw new ArgumentNullException(nameof(inventoryRepository));
        }

        public void CreateDashboard(string name, DateTime startDate, DateTime endDate, int validityDuration)
        {
            var newDashboard = new InventoryDashboardRDM(name, startDate, endDate, validityDuration);
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
    }
}