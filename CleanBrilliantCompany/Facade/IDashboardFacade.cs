using CleanBrilliantCompany.Control;
using CleanBrilliantCompany.Interfaces.Forecast;
public interface IDashboardFacade
{

    string GenerateCombinedReport(List<string> selected);

}