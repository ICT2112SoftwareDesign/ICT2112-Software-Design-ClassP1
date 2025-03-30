using CleanBrilliantCompany.Control;
public interface IDashboardFacade
{
    List<Dashboard> getDashboardsData();

    AgingControl GetAgingControl();
    ManufacturerControl GetManufacturerControl();
    CostControl GetCostControl();
    InventoryControl GetInventoryControl();
}