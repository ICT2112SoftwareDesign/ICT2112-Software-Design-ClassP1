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
            dashboard.GeneratedDate = DateTime.Now;
            _context.InventoryDashboards.Add(dashboard);
            _context.SaveChanges();
        }

        public InventoryDashboardRDM GetLatestDashboard()
        {
            return _context.InventoryDashboards
                .OrderByDescending(d => d.GeneratedDate)
                .FirstOrDefault();
        }

        public InventoryDashboardRDM GetDashboardById(int id)
        {
            return _context.InventoryDashboards
                .FirstOrDefault(d => d.DashboardId == id);
        }
    }
}