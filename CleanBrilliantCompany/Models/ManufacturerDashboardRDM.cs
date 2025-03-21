using System;
using System.Collections.Generic;
using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Models
{
    // ManufacturerDashboard class inherits from Dashboard
    public class ManufacturerDashboard : Dashboard
    {
        // New attributes specific to the ManufacturerDashboard
        public Dictionary<int, float> PunctualDeliveryRate { get; set; }
        public Dictionary<int, float> AverageDeliveryLeadTime { get; set; }
        public Dictionary<int, float> DefectRate { get; set; }
        public Dictionary<int, float> DependencyPercentage { get; set; }
        public Dictionary<int, float> ManufacturerScore { get; set; }
        public Dictionary<int, List<float>> ManufacturerScoreTrend { get; set; }
        public Dictionary<int, bool> RiskFlag { get; set; }

        // Constructor
        public ManufacturerDashboard(string name, DateTime requestedStartDate, DateTime requestedEndDate, int validityDuration, DashboardType type)
            : base(0, name, requestedStartDate, requestedEndDate, DateTime.Now, validityDuration, type)
        {
            PunctualDeliveryRate = new Dictionary<int, float>();
            AverageDeliveryLeadTime = new Dictionary<int, float>();
            DefectRate = new Dictionary<int, float>();
            DependencyPercentage = new Dictionary<int, float>();
            ManufacturerScore = new Dictionary<int, float>();
            ManufacturerScoreTrend = new Dictionary<int, List<float>>();
            RiskFlag = new Dictionary<int, bool>();
        }

        // Method to add or update punctual delivery rate for a specific manufacturer
        public void SetPunctualDeliveryRate(int manufacturerId, float rate)
        {
            if (PunctualDeliveryRate.ContainsKey(manufacturerId))
            {
                PunctualDeliveryRate[manufacturerId] = rate;
            }
            else
            {
                PunctualDeliveryRate.Add(manufacturerId, rate);
            }
        }

        // Method to add or update average delivery lead time for a specific manufacturer
        public void SetAverageDeliveryLeadTime(int manufacturerId, float leadTime)
        {
            if (AverageDeliveryLeadTime.ContainsKey(manufacturerId))
            {
                AverageDeliveryLeadTime[manufacturerId] = leadTime;
            }
            else
            {
                AverageDeliveryLeadTime.Add(manufacturerId, leadTime);
            }
        }

        // Method to add or update defect rate for a specific manufacturer
        public void SetDefectRate(int manufacturerId, float defectRate)
        {
            if (DefectRate.ContainsKey(manufacturerId))
            {
                DefectRate[manufacturerId] = defectRate;
            }
            else
            {
                DefectRate.Add(manufacturerId, defectRate);
            }
        }

        // Method to add or update dependency percentage for a specific manufacturer
        public void SetDependencyPercentage(int manufacturerId, float percentage)
        {
            if (DependencyPercentage.ContainsKey(manufacturerId))
            {
                DependencyPercentage[manufacturerId] = percentage;
            }
            else
            {
                DependencyPercentage.Add(manufacturerId, percentage);
            }
        }

        // Method to add or update manufacturer score for a specific manufacturer
        public void SetManufacturerScore(int manufacturerId, float score)
        {
            if (ManufacturerScore.ContainsKey(manufacturerId))
            {
                ManufacturerScore[manufacturerId] = score;
            }
            else
            {
                ManufacturerScore.Add(manufacturerId, score);
            }
        }

        // Method to add or update manufacturer score trend for a specific manufacturer
        public void SetManufacturerScoreTrend(int manufacturerId, List<float> scoreTrend)
        {
            if (ManufacturerScoreTrend.ContainsKey(manufacturerId))
            {
                ManufacturerScoreTrend[manufacturerId] = scoreTrend;
            }
            else
            {
                ManufacturerScoreTrend.Add(manufacturerId, scoreTrend);
            }
        }

        // Method to add or update risk flag for a specific manufacturer
        public void SetRiskFlag(int manufacturerId, bool isRisk)
        {
            if (RiskFlag.ContainsKey(manufacturerId))
            {
                RiskFlag[manufacturerId] = isRisk;
            }
            else
            {
                RiskFlag.Add(manufacturerId, isRisk);
            }
        }

        // Overriding abstract method DisplayDashboard to implement specific behavior
        public override void DisplayDashboard()
        {
            Console.WriteLine($"Displaying Manufacturer Dashboard: {Name}");
            // Logic to display the Manufacturer Dashboard with additional info
            Console.WriteLine($"Punctual Delivery Rates: {PunctualDeliveryRate.Count} manufacturers");
            Console.WriteLine($"Average Delivery Lead Times: {AverageDeliveryLeadTime.Count} manufacturers");
            Console.WriteLine($"Defect Rates: {DefectRate.Count} manufacturers");
            Console.WriteLine($"Dependency Percentages: {DependencyPercentage.Count} manufacturers");
            Console.WriteLine($"Manufacturer Scores: {ManufacturerScore.Count} manufacturers");
            Console.WriteLine($"Risk Flags: {RiskFlag.Count} manufacturers");
        }
    }
}