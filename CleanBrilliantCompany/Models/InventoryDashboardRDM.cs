using System;
using System.Collections.Generic;
using System.Data;

namespace CleanBrilliantCompany.Models
{
    public class InventoryDashboardRDM : Dashboard
    {
        private readonly Dictionary<int, int> _stockLevel;
        private readonly Dictionary<int, int> _threshold;
        private readonly Dictionary<int, bool> _replenishmentStatus;

        // Public properties for EF Core to access, but with private setters
        public Dictionary<int, int> StockLevel
        {
            get => new Dictionary<int, int>(_stockLevel);
            private set => throw new InvalidOperationException("Use SetStockLevel to modify StockLevel.");
        }

        public Dictionary<int, int> Threshold
        {
            get => new Dictionary<int, int>(_threshold);
            private set => throw new InvalidOperationException("Use SetThreshold to modify Threshold.");
        }

        public Dictionary<int, bool> ReplenishmentStatus
        {
            get => new Dictionary<int, bool>(_replenishmentStatus);
            private set => throw new InvalidOperationException("Use SetReplenishmentStatus to modify ReplenishmentStatus.");
        }

        public InventoryDashboardRDM(string name, int validityDuration)
            : base(name, DateTime.Now, DateTime.Now, validityDuration)
        {
            _stockLevel = new Dictionary<int, int>();
            _threshold = new Dictionary<int, int>();
            _replenishmentStatus = new Dictionary<int, bool>();
        }

        public InventoryDashboardRDM(int dashboardId, string name, DateTime requestedStartDate, DateTime requestedEndDate, int validityDuration, DateTime generatedDate)
            : base(dashboardId, name, requestedStartDate, requestedEndDate, validityDuration, 2, generatedDate)
        {
            _stockLevel = new Dictionary<int, int>();
            _threshold = new Dictionary<int, int>();
            _replenishmentStatus = new Dictionary<int, bool>();
        }

        public int GetDashboardId()
        {
            return DashboardId;
        }

        private void EnsureProductExists(int productId)
        {
            // If the product doesn't exist, initialize it in all dictionaries
            if (!_stockLevel.ContainsKey(productId))
            {
                _stockLevel[productId] = 0; // Default stock level
            }
            if (!_threshold.ContainsKey(productId))
            {
                _threshold[productId] = 0; // Default threshold
            }
            if (!_replenishmentStatus.ContainsKey(productId))
            {
                _replenishmentStatus[productId] = false; // Default replenishment status
            }
        }

        public int GetStockLevel(int productId)
        {
            return _stockLevel.TryGetValue(productId, out var stock) ? stock : -1;
        }

        public Dictionary<int, int> GetAllStockLevels()
        {
            return new Dictionary<int, int>(_stockLevel);
        }

        public int GetThreshold(int productId)
        {
            return _threshold.TryGetValue(productId, out var stock) ? stock : -1;
        }

        public Dictionary<int, int> GetAllThresholds()
        {
            return new Dictionary<int, int>(_threshold);
        }

        public bool? GetReplenishmentStatus(int productId)
        {
            return _replenishmentStatus.TryGetValue(productId, out var status) ? status : null;
        }

        public Dictionary<int, bool> GetAllReplenishmentStatuses()
        {
            return new Dictionary<int, bool>(_replenishmentStatus);
        }

        protected void SetStockLevel(int productId, int stockLevel)
        {
            EnsureProductExists(productId);
            _stockLevel[productId] = stockLevel;
        }

        protected void SetThreshold(int productId, int threshold)
        {
            EnsureProductExists(productId);
            _threshold[productId] = threshold;
        }

        protected void SetReplenishmentStatus(int productId, bool replenishmentStatus)
        {
            EnsureProductExists(productId);
            _replenishmentStatus[productId] = replenishmentStatus;
        }

        public string GetStockStatus(int productId)
        {
            EnsureProductExists(productId);
            var stockLevel = _stockLevel[productId];
            var threshold = _threshold[productId];

            if (stockLevel < threshold * 0.35)
            {
                return "L"; // Low Stock
            }
            else if (stockLevel > threshold * 1.6)
            {
                return "O"; // Over Stock
            }
            else
            {
                return "N"; // Normal
            }
        }

        public Dictionary<int, string> GetAllStockStatuses()
        {
            var statuses = new Dictionary<int, string>();
            foreach (var productId in _stockLevel.Keys)
            {
                statuses[productId] = GetStockStatus(productId);
            }
            return statuses;
        }

        public bool IsLowStock(int productId)
        {
            return _stockLevel[productId] < _threshold[productId] * 0.35;
        }

        public bool IsOverStock(int productId)
        {
            return _stockLevel[productId] > _threshold[productId] * 1.6;
        }

        public bool NeedsReplenishment(int productId)
        {
            return _stockLevel[productId] < _threshold[productId] * 0.35; // DOUBLE CHECK THIS LATER!!
        }

        public void UpdateDashboardData(Dictionary<int, int> stockLevels, Dictionary<int, int> thresholds)
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

        public void UpdateReplenishmentStatus()
        {
            foreach (var productId in _stockLevel.Keys)
            {
                bool needsReplenishment = _stockLevel[productId] < _threshold[productId] * 0.35;
                SetReplenishmentStatus(productId, needsReplenishment);
            }
        }

    }
}