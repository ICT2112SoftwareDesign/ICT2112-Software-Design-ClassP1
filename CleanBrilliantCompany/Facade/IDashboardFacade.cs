using CleanBrilliantCompany.Control;
using CleanBrilliantCompany.Interfaces.Forecast;
public interface IDashboardFacade
{
    List<Dashboard> getDashboardsData();

    string GenerateCombinedReport(List<string> selected);

}