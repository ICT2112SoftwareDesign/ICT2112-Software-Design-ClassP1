using System;
using System.Collections.Generic;
using System.Linq;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Control
{
    public class InventoryControl
    {
        private readonly InventoryDashboardRDM _inventoryDashboard;

        public InventoryControl(InventoryDashboardRDM inventoryDashboard)
        {
            _inventoryDashboard = inventoryDashboard ?? throw new ArgumentNullException(nameof(inventoryDashboard));
        }

        public InventoryDashboardRDM FetchDashboard()
        {
            return _inventoryDashboard;
        }

        public void UpdateDashboard(Dictionary<int, int> stockUpdates)
        {
            // Fetch latest stock data
            Dictionary<int, int> latestStockLevels = FetchLatestStockLevels();
            Dictionary<int, int> latestThresholds = FetchLatestThresholds();

            // Update InventoryDashboardRDM with latest values
            _inventoryDashboard.updateDashboardData(latestStockLevels, latestThresholds);

            // Recalculate replenishment statuses
            _inventoryDashboard.updateReplenishmentStatus();

            Console.WriteLine("Dashboard updated with latest stock and threshold values.");
        }

        private Dictionary<int, int> FetchLatestStockLevels()
        {
            return new Dictionary<int, int>(_inventoryDashboard.GetAllStockLevels());
        }

        private Dictionary<int, int> FetchLatestThresholds()
        {
            return new Dictionary<int, int>(_inventoryDashboard.GetAllThresholds());
        }

        public List<int> CheckLowStock()
        {
            return _inventoryDashboard
                .GetAllStockLevels()
                .Where(stock => stock.Value < _inventoryDashboard.GetThreshold(stock.Key))
                .Select(stock => stock.Key)
                .ToList();
        }

        public List<int> CheckOverStock()
        {
            return _inventoryDashboard
                .GetAllStockLevels()
                .Where(stock => stock.Value > _inventoryDashboard.GetThreshold(stock.Key) * 1.5)
                .Select(stock => stock.Key)
                .ToList();
        }

        public List<int> GenerateReplenishmentActions()
        {
            return CheckLowStock(); // Replenishment actions = all low-stock products
        }

        public List<int> PrioritiseReplenishment()
        {
            return CheckLowStock()
                .OrderBy(productId => _inventoryDashboard.GetStockLevel(productId))
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