using System;
using CleanBrilliantCompany.DataSource.Interface;
using CleanBrilliantCompany.DbContextEntities;
using CleanBrilliantCompany.ForecastManagement.Models;
using CleanBrilliantCompany.Models.Forecast;
using Microsoft.Identity.Client;

namespace CleanBrilliantCompany.DataSource.Mapper
{
    public class ForecastMapper : IForecastRepository
    {
        private readonly ApplicationDbContext _context;

        public ForecastMapper(ApplicationDbContext context)
        {
            _context = context;
        }
        
  
        public void saveDashboard(ForecastDashboard dashboard)
        {
            ForecastDashboardTable dashboardDto = toDTO(dashboard);
            _context.ForecastDashboards.Add(dashboardDto);
            _context.SaveChanges();

            foreach (ForecastMetrics metric in dashboard.GetMetrics()){
                ForecastMetricTable metricDto = toDTO(metric, dashboardDto.DashBoardID);
                _context.ForecastMetrics.Add(metricDto);
            }
            _context.SaveChanges();

        }
        public ForecastDashboard getDashboard(int month,int year)
        {

          

            var dashboardDto = _context.ForecastDashboards
                    .FirstOrDefault(d => d.StartDate.Month == month && d.StartDate.Year == year && d.GeneratedDateTime.Year==DateTime.Now.Year);
            
            if (dashboardDto == null)
            {
                // No dashboard exists for this month/year.
                return null; // or you could create a new dashboard if desired.
            }

            var metricDtos = _context.ForecastMetrics
                .Where(m => m.DashboardID == dashboardDto.DashBoardID)
               .ToList();
                    _context.SaveChanges();

            var metrics = new List<ForecastMetrics>();
            foreach (var metricDto in metricDtos)
            {
                // Assume you have a helper method that converts a MetricDTO to a ForecastMetrics
                ForecastMetrics metric = toEntity(metricDto);
                metrics.Add(metric);
            }

            // Convert the dashboard DTO (and its metrics) to the domain model.
            ForecastDashboard dashboard = toEntity(dashboardDto, metrics);
            return dashboard;
        }
        //public void CreateDashboard(ForecastDashboard dashboard)
        //{
        //    ForecastDashboardDTO dto = toDTO(dashboard);
        //    _context.ForecastDashboards.Add(dto);
        //}
        //public ForecastDashboard getDashboard()
        //{
        //    ForecastDashboardDTO dto = _context.ForecastDashboards.FirstOrDefault();
        //    if (dto != null)
        //    {
        //        return toEntity(dto); //need to accept forecast DTO also
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        public ForecastDashboardTable toDTO(ForecastDashboard dashboard)
        {
            int dashBoardId = dashboard.GetDashBoardID();
            DateTime startDate = dashboard.GetStartDate();
            DateTime endDate = dashboard.GetEndDate();
            DateTime generatedDate= dashboard.GetGeneratedDate();
            int validityDuration = dashboard.GetValidityDuration();
            return new ForecastDashboardTable(dashBoardId, startDate, endDate, generatedDate);
        }
        public ForecastDashboard toEntity(ForecastDashboardTable dto, List<ForecastMetrics> metrics)
        {
            int dashBoardId = dto.DashBoardID;
            DateTime startDate = dto.StartDate;
            DateTime endDate = dto.EndDate;
            DateTime generatedDate = dto.GeneratedDateTime;

            return new ForecastDashboard(dashBoardId, startDate, endDate, generatedDate, 0, metrics);
        }
        private ForecastMetricTable toDTO(ForecastMetrics forecastMetrics, int dashboardID)
        {
            int metricId = forecastMetrics.getMetricId();
            int productId= forecastMetrics.getProductId();
            int forecastedStock = forecastMetrics.getForecastedStock();
            return new ForecastMetricTable(dashboardID,metricId,  productId,forecastedStock );
        }
        private ForecastMetrics toEntity(ForecastMetricTable dto)
        {
            return new StockForecast( dto.ProductID, dto.ForecastedStock);
        }
    }
}

