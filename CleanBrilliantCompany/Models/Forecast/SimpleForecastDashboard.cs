using System;
using System.Collections.Generic;

namespace CleanBrilliantCompany.Models
{
    public class SimpleForecastDashboard
    {
        // Properties
        private int DashBoardID { get; set; }
        private DateTime StartDate { get; set; }
        private DateTime EndDate { get; set; }
        private DateTime GeneratedDate { get; set; }
        private int ValidityDuration { get; set; }
        private List<ForecastMetrics> Metrics { get; set; } = new List<ForecastMetrics>();

        // Methods
        public int GetDashBoardID()
        {
            return DashBoardID;
        }

        public void SetDashBoardID(int dashBoardID)
        {
            DashBoardID = dashBoardID;
        }

        public DateTime GetStartDate()
        {
            return StartDate;
        }

        public void SetStartDate(DateTime startDate)
        {
            StartDate = startDate;
        }

        public DateTime GetEndDate()
        {
            return EndDate;
        }

        public void SetEndDate(DateTime endDate)
        {
            EndDate = endDate;
        }

        public DateTime GetGeneratedDate()
        {
            return GeneratedDate;
        }

        public void SetGeneratedDate(DateTime generatedDate)
        {
            GeneratedDate = generatedDate;
        }

        public int GetValidityDuration()
        {
            return ValidityDuration;
        }

        public void SetValidityDuration(int validityDuration)
        {
            ValidityDuration = validityDuration;
        }

        public List<ForecastMetrics> GetMetrics()
        {
            return Metrics;
        }

        public void SetMetrics(List<ForecastMetrics> metrics)
        {
            Metrics = metrics;
        }

        // Methods
        public void GetDashboard()
        {
            // Implementation logic to retrieve or display dashboard details
        }

        public void ComputeMatrix()
        {
            // Logic to compute or generate matrix-related calculations
        }
    }

}
