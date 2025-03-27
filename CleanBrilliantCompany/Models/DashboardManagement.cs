using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Models
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

        public DashboardSummary GetDashboardSummary()
        {
            return new DashboardSummary
            {
                TotalOrders = _orderDatabase.GetTotalOrderCount(),
                TotalRefunds = _refundDatabase.GetTotalRefundCount(),
                NetRevenue = _orderDatabase.GetTotalOrderValue() - _refundDatabase.GetTotalRefundAmount(),
                PendingRefunds = _refundDatabase.GetPendingRefundCount()
            };
        }
    }

    public class DashboardSummary
    {
        public int TotalOrders { get; set; }
        public int TotalRefunds { get; set; }
        public decimal NetRevenue { get; set; }
        public int PendingRefunds { get; set; }
    }
}