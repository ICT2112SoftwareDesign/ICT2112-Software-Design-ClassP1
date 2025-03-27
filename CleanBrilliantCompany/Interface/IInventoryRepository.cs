using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Interface
{
    public interface IInventoryRepository
    {
        void SaveDashboard(InventoryDashboardRDM dashboard);
        InventoryDashboardRDM GetLatestDashboard();
        //Dictionary<int, (int LowStockCount, int OverStockCount)> GetAlertCounts();

        Dictionary<int, (int LowStockWeeks, int OverStockWeeks)> GetConsecutiveWeeklyAlerts();
    }
}