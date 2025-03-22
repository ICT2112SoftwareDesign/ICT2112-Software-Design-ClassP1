using System;
using CleanBrilliantCompany.DataSource.Interface;
using CleanBrilliantCompany.DTO;
using CleanBrilliantCompany.Models.Forecast;

namespace CleanBrilliantCompany.DataSource.Mapper
{
    public class ForecastMapper : IForecastRepository
    {
        private readonly ApplicationDbContext _context;

        public ForecastMapper(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public ForecastDashboard getLatestDashboard()
        {
            DateTime startDate = DateTime.Now.AddDays(-7); // Start date 7 days ago
            DateTime endDate = DateTime.Now;
            List<ForecastMetrics> metricsList = new List<ForecastMetrics>
            {
              new StockForecast(1,12,"A"),
              new StockForecast(2,3, "B")
            };
            ForecastDashboard forecastDashboard = new ForecastDashboard(0, startDate, endDate, DateTime.Now, 0, metricsList);
            return forecastDashboard;
        }

        public void CreateDashboard(ForecastDashboard dashboard)
        {
            ForecastDashboardDTO dto = toDTO(dashboard);
            _context.ForecastDashboards.Add(dto);
        }
        public ForecastDashboard getDashboard()
        {
            ForecastDashboardDTO dto = _context.ForecastDashboards.FirstOrDefault();
            if (dto != null)
            {
                return toEntity(dto); //need to accept forecast DTO also
            }
            else
            {
                return null;
            }
        }
        public ForecastDashboardDTO toDTO(ForecastDashboard dashboard)
        {
            int dashBoardId = dashboard.GetDashBoardID();
            DateTime startDate = dashboard.GetStartDate();
            DateTime endDate = dashboard.GetEndDate();
            DateTime generatedDate= dashboard.GetGeneratedDate();
            int validityDuration = dashboard.GetValidityDuration();
            return new ForecastDashboardDTO(dashBoardId, startDate, endDate, generatedDate);
        }
        public ForecastDashboard toEntity(ForecastDashboardDTO dto)
        {
            int dashBoardId = dto.DashBoardID;
            DateTime startDate = dto.StartDate;
            DateTime endDate = dto.EndDate;
            DateTime generatedDate = dto.GeneratedDateTime;

            List<ForecastMetrics> metrics = [new StockForecast()];//TO BE IMPLEMENTED
            return new ForecastDashboard(dashBoardId, startDate, endDate, generatedDate, 0, metrics);
        }
    }
}

