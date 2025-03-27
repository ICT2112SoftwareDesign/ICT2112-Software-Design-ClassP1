using System;
using System.Collections.Generic;
using System.Linq;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Management
{
    public class DashboardManagement
    {
        private readonly IOrderDatabase _orderDatabase;
        private readonly IRefundDatabase _refundDatabase;

        public DashboardManagement(IOrderDatabase orderDatabase, IRefundDatabase refundDatabase)
        {
            _orderDatabase = orderDatabase;
            _refundDatabase = refundDatabase;
        }

        public (int TotalOrders, int TotalRefunds, decimal NetRevenue, int PendingRefunds) GetDashboardMetrics()
        {
            int totalOrders = _orderDatabase.GetTotalOrderCount();
            int totalRefunds = _refundDatabase.GetTotalRefundCount();
            decimal netRevenue = _orderDatabase.GetTotalOrderValue() - _refundDatabase.GetTotalRefundAmount();
            int pendingRefunds = _refundDatabase.GetPendingRefundCount();

            return (totalOrders, totalRefunds, netRevenue, pendingRefunds);
        }
    }
}