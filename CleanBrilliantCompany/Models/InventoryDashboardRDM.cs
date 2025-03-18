using System;
using System.Collections.Generic;

namespace CleanBrilliantCompany.Models
{
    public class InventoryDashboardRDM : Dashboard
    {
        private Dictionary<int, int> StockLevel;
        private Dictionary<int, int> Threshold;
        private Dictionary<int, bool?> ReplenishmentStatus;

        public InventoryDashboardRDM(string name, DateTime requestedStartDate, DateTime requestedEndDate, int validityDuration) : base(name, requestedStartDate, requestedEndDate, validityDuration)
        {


            StockLevel = new Dictionary<int, int>();
            Threshold = new Dictionary<int, int>();
            ReplenishmentStatus = new Dictionary<int, bool?>();
        }

        private void EnsureProductExists(int productId)
        {
            if (!StockLevel.ContainsKey(productId) || !Threshold.ContainsKey(productId) || !ReplenishmentStatus.ContainsKey(productId))
            {
                throw new InvalidOperationException($"Product {productId} does not exist.");
            }
        }

        // get methods
        public int GetStockLevel(int productId)
        {
            return StockLevel.TryGetValue(productId, out var stock) ? stock : -1; // Return -1 if product is not found
        }

        public Dictionary<int, int> GetAllStockLevels()
        {
            return new Dictionary<int, int>(StockLevel);
        }

        public int GetThreshold(int productId)
        {
            return Threshold.TryGetValue(productId, out var stock) ? stock : -1; // Return -1 if product is not found
        }

        public Dictionary<int, int> GetAllThresholds()
        {
            return new Dictionary<int, int>(Threshold);
        }

        public bool? GetReplenishmentStatus(int productId)
        {
            return ReplenishmentStatus.TryGetValue(productId, out var status) ? status : (bool?)null;
        }

        public Dictionary<int, bool?> GetAllReplenishmentStatuses()
        {
            return new Dictionary<int, bool?>(ReplenishmentStatus);
        }

        // set methods
        protected void SetStockLevel(int productId, int stockLevel)
        {
            EnsureProductExists(productId);
            StockLevel[productId] = stockLevel;

            // UpdateReplenishmentStatus(productId);
        }

        protected void SetThreshold(int productId, int threshold)
        {
            EnsureProductExists(productId);
            Threshold[productId] = threshold;
        }

        protected void SetReplenishmentStatus(int productId, bool replenishmentStatus)
        {
            EnsureProductExists(productId);
            ReplenishmentStatus[productId] = replenishmentStatus;
        }

        public bool isLowStock(int productId)
        {
            return StockLevel[productId] < Threshold[productId];
        }

        public bool isOverStock(int productId)
        {
            return StockLevel[productId] > Threshold[productId] * 1.5;
        }

        public bool needsReplenishment(int productId)
        {
            return StockLevel[productId] < Threshold[productId];
        }

        // public List<Alert> generateAlerts()
        // {
        //     List<Alert> alerts = new List<Alert>();
        //     foreach (var item in StockLevel)
        //     {
        //         if (item.Value < Threshold[item.Key])
        //         {
        //             alerts.Add(new Alert(item.Key, "Low stock"));
        //         }
        //     }
        //     return alerts;
        // }

        public void updateDashboardData(Dictionary<int, int> stockLevels, Dictionary<int, int> thresholds)
        {
            foreach (var item in stockLevels)
            {
                SetStockLevel(item.Key, item.Value);
            }

            foreach (var item in thresholds)
            {
                SetThreshold(item.Key, item.Value);
            }
        }

        public void updateReplenishmentStatus()
        {
            foreach (var productId in StockLevel.Keys)
            {
                bool needsReplenishment = StockLevel[productId] < Threshold[productId];
                SetReplenishmentStatus(productId, needsReplenishment);
            }
        }

    }
}