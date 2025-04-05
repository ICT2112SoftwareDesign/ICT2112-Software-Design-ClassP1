using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using CleanBrilliantCompany.Interfaces.Forecast;

namespace CleanBrilliantCompany.ForecastManagement.Models
{
    public class ForecastDashboard
    {
        // Properties
        [JsonInclude]
        [JsonPropertyName("DashBoardID")]
        private int dashBoardID { get; set; }

        [JsonPropertyName("StartDate")]
        private DateTime startDate { get; set; }

        [JsonInclude]
        [JsonPropertyName("EndDate")]
        private DateTime endDate { get; set; }

        [JsonInclude]
        [JsonPropertyName("GeneratedDate")]
        private DateTime generatedDate { get; set; }

        [JsonInclude]
        [JsonPropertyName("ValidityDuration")]
        private int validityDuration { get; set; }

        [JsonInclude]
        [JsonPropertyName("MetricsList")]
        private List<ForecastMetrics> metricsList { get; set; } = new();

        [JsonInclude]
        [JsonPropertyName("AlertItemList")]
        private List<string> alertItemList { get; set; } = new();

        public ForecastDashboard()
        {
        }
        public ForecastDashboard(int dashboardID, DateTime startDate, DateTime endDate, DateTime generatedDate, int validityDuration, List<ForecastMetrics> metricsList)
        {
            DashBoardID = dashboardID;
            StartDate = startDate;
            EndDate = endDate;
            GeneratedDate = generatedDate;
            ValidityDuration = validityDuration;
            MetricsList = metricsList;

        }

        // Methods
        public int GetDashBoardID()
        {
            return DashBoardID;
        }

        private void SetDashBoardID(int dashBoardID)
        {
            DashBoardID = dashBoardID;
        }

        public DateTime GetStartDate()
        {
            return StartDate;
        }

        private void SetStartDate(DateTime startDate)
        {
            StartDate = startDate;
        }

        public DateTime GetEndDate()
        {
            return EndDate;
        }

        private void SetEndDate(DateTime endDate)
        {
            EndDate = endDate;
        }

        public DateTime GetGeneratedDate()
        {
            return GeneratedDate;
        }

        private void SetGeneratedDate(DateTime generatedDate)
        {
            GeneratedDate = generatedDate;
        }

        public int GetValidityDuration()
        {
            return ValidityDuration;
        }

        private void SetValidityDuration(int validityDuration)
        {
            ValidityDuration = validityDuration;
        }

        public List<ForecastMetrics> GetMetrics()
        {
            return MetricsList;
        }

        public void SetMetrics(List<ForecastMetrics> metrics)
        {
            MetricsList = metrics;
        }



        // Methods
        public ForecastDashboard GetDashboard()
        {
            return this;
        }
        public void AddMetric(ForecastMetrics metric)
        {
            if (metric != null)
            {
                MetricsList.Add(metric);
            }
        }
        public void UpdateMetric(ForecastMetrics updatedMetric)
        {
            for (int i = 0; i < MetricsList.Count; i++)
            {
                if (MetricsList[i].GetProductID() == updatedMetric.GetProductID())
                {
                    // Replace the old metric with the updated one at the same index
                    MetricsList[i] = updatedMetric;
                }
            }
        }


        public void DeleteMetric(int productId)
        {
            MetricsList.RemoveAll(metric => metric.GetProductID() == productId);
        }

        public List<string> GetAlertItemList()
        {
            return AlertItemList;
        }
        public void SetAlertItemList(List<string> list)
        {
            AlertItemList = list;
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
