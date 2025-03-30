using CleanBrilliantCompany.Control;
using CleanBrilliantCompany.Interfaces.Forecast;
public interface IDashboardFacade
{
    List<Dashboard> getDashboardsData();

    AgingControl GetAgingControl();
    ManufacturerControl GetManufacturerControl();
    CostControl GetCostControl();
    InventoryControl GetInventoryControl();
    IForecastControl GetForecastControl();
}