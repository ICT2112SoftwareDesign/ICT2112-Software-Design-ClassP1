public interface IInventoryRepository
{
    void SaveDashboard(InventoryDashboardRDM dashboard);
    InventoryDashboardRDM GetLatestDashboard();
    Dictionary<int, (int LowStockWeeks, int OverStockWeeks)> GetConsecutiveWeeklyAlerts();
    Dictionary<int, int?> GetAllProductThresholds();
}
