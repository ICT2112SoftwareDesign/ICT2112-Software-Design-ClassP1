using CleanBrilliantCompany.Control;
using CleanBrilliantCompany.ForecastManagement.Interface;

public interface IDashboardFacade
{
    List<Dashboard> getDashboardsData();

    AgingControl GetAgingControl();
    ManufacturerControl GetManufacturerControl();
    CostControl GetCostControl();
    InventoryControl GetInventoryControl();
    IForecastReportDetails GetForecastControl();
}