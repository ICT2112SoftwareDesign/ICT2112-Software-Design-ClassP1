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

        public void SaveDashboard(InventoryDashboardRDM dashboard)
        {
            // Map the business model (InventoryDashboardRDM) to the entity (DashboardEntity)
            var dashboardEntity = new DashboardTable
            {
                Name = dashboard.Name,
                RequestedStartDate = dashboard.RequestedStartDate,
                RequestedEndDate = dashboard.RequestedEndDate,
                GeneratedDate = DateTime.Now,
                ValidityDuration = dashboard.ValidityDuration,
                TypeId = dashboard.Type
            };

            _context.DashboardTable.Add(dashboardEntity);
            _context.SaveChanges();

            // Update the DashboardId in the business model
            typeof(Dashboard).GetProperty("DashboardId")
                ?.SetValue(dashboard, dashboardEntity.DashboardId, null);
        }

        public InventoryDashboardRDM? GetLatestDashboard()
        {
            var dashboardEntity = _context.DashboardTable // Fixed: Changed Dashboards to DashboardTable
                .Include(d => d.InventoryLevels)
                .ThenInclude(i => i.StockStatus)
                .Include(d => d.InventoryLevels)
                .ThenInclude(i => i.AlertTypes)
                .ThenInclude(a => a.AlertType)
                .OrderByDescending(d => d.GeneratedDate)
                .FirstOrDefault();

            if (dashboardEntity == null)
                return null;

            return new InventoryDashboardRDM(
                dashboardEntity.DashboardId,
                dashboardEntity.Name,
                dashboardEntity.RequestedStartDate,
                dashboardEntity.RequestedEndDate,
                dashboardEntity.ValidityDuration,
                dashboardEntity.GeneratedDate);
        }
    }
}