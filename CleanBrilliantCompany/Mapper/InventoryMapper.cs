using System;
using System.Collections.Generic;
using System.Linq;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Interface;
using CleanBrilliantCompany.Data;

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
            //dashboard.GeneratedDate = DateTime.Now;
            //_context.InventoryLevel.Add(dashboard);
            //_context.SaveChanges();

            //if (dashboard.GetDashboardId() == 0)
            //{
            //    var maxId = _context.InventoryLevel.Any()
            //        ? _context.InventoryLevel.Max(d => d.DashboardId)
            //        : 0;
            //    typeof(Dashboard).GetProperty("DashboardId")?.SetValue(dashboard, maxId + 1);
            //}

            //dashboard.GeneratedDate = DateTime.Now;
            //_context.InventoryLevel.Add(dashboard);
            //_context.SaveChanges();

            // Map the business model (InventoryDashboardRDM) to the entity (DashboardEntity)
            var dashboardEntity = new DashboardTable
            {
                Name = dashboard.Name,
                RequestedStartDate = dashboard.RequestedStartDate,
                RequestedEndDate = dashboard.RequestedEndDate,
                GeneratedDate = DateTime.Now, // Set directly on the entity
                ValidityDuration = dashboard.ValidityDuration,
                TypeId = dashboard.GetType().Name == "InventoryDashboardRDM" ? 1 : 0 // Verify Again!!
            };

            _context.DashboardTable.Add(dashboardEntity);
            _context.SaveChanges();

            // Update the DashboardId in the business model
            typeof(Dashboard).GetProperty("DashboardId")
                ?.SetValue(dashboard, dashboardEntity.DashboardId, null);
        }

        public InventoryDashboardRDM? GetLatestDashboard()
        {
            //return _context.InventoryLevel
            //    .OrderByDescending(d => d.GeneratedDate)
            //    .FirstOrDefault();

            var dashboardEntity = _context.Dashboards
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
                dashboardEntity.TypeId,
                dashboardEntity.GeneratedDate);
        }

        //public InventoryDashboardRDM GetDashboardById(int id)
        //{
        //    return _context.InventoryLevel
        //        .FirstOrDefault(d => d.DashboardId == id);
        //}
    }
}