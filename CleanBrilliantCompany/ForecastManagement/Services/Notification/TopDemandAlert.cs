using CleanBrilliantCompany.ForecastManagement.Interface;
using CleanBrilliantCompany.ForecastManagement.Models;

namespace CleanBrilliantCompany.Services.Notification
{
    public class TopDemandAlert : IAlert
    {
        public List<string> alert(List<ForecastMetrics> metrics)
        {
            var topMetrics = metrics
               .OrderByDescending(m => m.getForecastedStock())
                .Take(4)
                .Select(m => $"{m.getProductName()} (ID: {m.getProductID()})").ToList();

            return topMetrics;
        }



    }
}
