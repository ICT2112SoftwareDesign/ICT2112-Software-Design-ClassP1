using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Interface
{
    public interface IInventoryRepository
    {
        void SaveDashboard(InventoryDashboardRDM dashboard);
        InventoryDashboardRDM GetLatestDashboard();
        //InventoryDashboardRDM GetDashboardById(int id);

    }
}