using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Interface;
using CleanBrilliantCompany.Data;
using CleanBrilliantCompany.Entities;

namespace CleanBrilliantCompany.Mapper
{
    public class InventoryMapper : IInventoryRepository
    {
        private readonly AppDbContext _context;

        public InventoryMapper(AppDbContext context)
        {
            _context = context;
        }

        private StockStatusTable GetStockStatus(string stockCode)
        {
            var stockStatus = _context.StockStatusTable.FirstOrDefault(s => s.StockCode == stockCode);
            if (stockStatus == null)
            {
                throw new InvalidOperationException($"Stock status with code {stockCode} not found in StockStatus table.");
            }
            return stockStatus;
        }

        public void SaveDashboard(InventoryDashboardRDM dashboard)
        {
            var now = DateTime.Now;

            // Map the business model (InventoryDashboardRDM) to the entity (DashboardEntity)
            var dashboardEntity = new DashboardTable
            {
                Name = dashboard.Name,
                RequestedStartDate = now,
                RequestedEndDate = now,
                GeneratedDate = now,
                ValidityDuration = dashboard.ValidityDuration,
                TypeId = dashboard.Type,
                InventoryLevels = new List<InventoryLevelTable>()
            };

            // Map stock levels, thresholds, replenishment status, and alerts to InventoryLevelTable and AlertTypeTable entities
            var stockLevels = dashboard.GetAllStockLevels();
            var thresholds = dashboard.GetAllThresholds();
            var replenishmentStatuses = dashboard.GetAllReplenishmentStatuses();
            var stockStatuses = dashboard.GetAllStockStatuses();

            foreach (var productId in stockLevels.Keys)
            {
                var stockCode = stockStatuses.ContainsKey(productId) ? stockStatuses[productId] : "N";
                var inventoryLevel = new InventoryLevelTable
                {
                    ProductId = productId,
                    StockLevel = stockLevels.ContainsKey(productId) ? stockLevels[productId] : 0,
                    Threshold = thresholds.ContainsKey(productId) ? thresholds[productId] : 0,
                    ReplenishmentStatus = replenishmentStatuses.ContainsKey(productId) ? replenishmentStatuses[productId] : false,
                    StockCode = stockCode,
                    StockStatus = GetStockStatus(stockCode)
                };
                dashboardEntity.InventoryLevels.Add(inventoryLevel);
            }

            _context.DashboardTable.Add(dashboardEntity);
            _context.SaveChanges();

            // Update the DashboardId in the business model
            typeof(Dashboard).GetProperty("DashboardId")
                ?.SetValue(dashboard, dashboardEntity.DashboardId, null);
        }

        public InventoryDashboardRDM? GetLatestDashboard()
        {
            var dashboardEntity = _context.DashboardTable
                .Include(d => d.InventoryLevels)
                .ThenInclude(i => i.StockStatus)
                .Where(t => t.TypeId == 2)
                .OrderByDescending(d => d.GeneratedDate)
                .FirstOrDefault();

            if (dashboardEntity == null)
                return null;

            var dashboard = new InventoryDashboardRDM(
                dashboardEntity.DashboardId,
                dashboardEntity.Name,
                dashboardEntity.RequestedStartDate,
                dashboardEntity.RequestedEndDate,
                dashboardEntity.ValidityDuration,
                dashboardEntity.GeneratedDate);

            // Populate stock levels, thresholds, and replenishment status from the database
            var stockLevels = new Dictionary<int, int>();
            var thresholds = new Dictionary<int, int>();
            var replenishmentStatuses = new Dictionary<int, bool>();

            foreach (var inventoryLevel in dashboardEntity.InventoryLevels)
            {
                stockLevels[inventoryLevel.ProductId] = inventoryLevel.StockLevel;
                thresholds[inventoryLevel.ProductId] = inventoryLevel.Threshold;
                replenishmentStatuses[inventoryLevel.ProductId] = inventoryLevel.ReplenishmentStatus;
            }

            dashboard.UpdateDashboardData(stockLevels, thresholds);
            dashboard.UpdateReplenishmentStatus();

            return dashboard;
        }
    }
}