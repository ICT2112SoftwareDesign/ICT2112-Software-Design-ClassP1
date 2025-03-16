using System;
using System.Collections.Generic;
using CleanBrilliantCompany.Interfaces.Forecast;

namespace CleanBrilliantCompany.Models.Forecast
{
    public class ForecastDashboard
    {
        // Properties
        private int DashBoardID { get; set; }
        private DateTime StartDate { get; set; }
        private DateTime EndDate { get; set; }
        private DateTime GeneratedDate { get; set; }
        private int ValidityDuration { get; set; }
        private List<ForecastMetrics> MetricsList { get; set; } = new List<ForecastMetrics>();

        private readonly IForecastingFacade _forecastingFacade;

        public ForecastDashboard(IForecastingFacade forecastingFacade)
        {
            _forecastingFacade = forecastingFacade;
        }

        public ForecastDashboard(DateTime startDate, DateTime endDate, List<ForecastMetrics> metricsList)
        {
            this.StartDate= startDate;
            this.EndDate= endDate;
            this.MetricsList = metricsList;
        }

        // Methods
        private int GetDashBoardID()
        {
            return DashBoardID;
        }

        private void SetDashBoardID(int dashBoardID)
        {
            DashBoardID = dashBoardID;
        }

        private DateTime GetStartDate()
        {
            return StartDate;
        }

        private void SetStartDate(DateTime startDate)
        {
            StartDate = startDate;
        }

        private DateTime GetEndDate()
        {
            return EndDate;
        }

        private void SetEndDate(DateTime endDate)
        {
            EndDate = endDate;
        }

        private DateTime GetGeneratedDate()
        {
            return GeneratedDate;
        }

        private void SetGeneratedDate(DateTime generatedDate)
        {
            GeneratedDate = generatedDate;
        }

        private int GetValidityDuration()
        {
            return ValidityDuration;
        }

        private void SetValidityDuration(int validityDuration)
        {
            ValidityDuration = validityDuration;
        }

        private List<ForecastMetrics> GetMetrics()
        {
            return MetricsList;
        }

        private void SetMetrics(List<ForecastMetrics> metrics)
        {
            MetricsList = metrics;
        }

        // Methods
        public ForecastDashboard GetDashboard()
        {
            return this;
        }

        //public void populateMetrics(String type)
        //{
        //    if (type == "stock")
        //    {
        //        this.Metrics.Add(_forecastingFacade.generateStockForecast());

        //    }
        //    else if (type == "price")
        //    {
        //        this.Metrics.Add(_forecastingFacade.generatePriceScenario());
        //    }
        //    // Logic to compute or generate matrix-related calculations
        //}
    }
}
