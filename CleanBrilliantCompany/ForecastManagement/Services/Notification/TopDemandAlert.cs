using CleanBrilliantCompany.ForecastManagement.Interface;
using CleanBrilliantCompany.ForecastManagement.Models;

namespace CleanBrilliantCompany.Services.Notification
{
    public class TopDemandAlert : IAlert
    {
        public List<string> Alert(List<ForecastMetrics> metrics)
        {
            var topMetrics = metrics
               .OrderByDescending(m => m.GetForecastedStock())
                .Take(4)
                .Select(m => $"{m.GetProductName()} (ID: {m.GetProductID()})").ToList();

            return topMetrics;
        }



    }
}
