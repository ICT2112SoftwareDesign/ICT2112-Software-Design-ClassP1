using System;

namespace CleanBrilliantCompany.Models
{
    // Assuming DashboardType is an enum defined elsewhere in your code
    public enum DashboardType
    {
        Aging,
        Inventory,
        Manufacturer
    }

    // Abstract class Dashboard
    public abstract class Dashboard
    {
        // Protected properties, to be accessible in derived classes
        protected int DashboardId { get; set; }
        protected string Name { get; set; }
        protected DateTime RequestedStartDate { get; set; }
        protected DateTime RequestedEndDate { get; set; }
        protected DateTime GeneratedDate { get; set; }
        protected int ValidityDuration { get; set; }
        protected DashboardType Type { get; set; }

        // Constructor
        public Dashboard(int dashboardId, string name, DateTime requestedStartDate, DateTime requestedEndDate, DateTime generatedDate, int validityDuration, DashboardType type)
        {
            DashboardId = dashboardId;
            Name = name;
            RequestedStartDate = requestedStartDate;
            RequestedEndDate = requestedEndDate;
            GeneratedDate = generatedDate;
            ValidityDuration = validityDuration;
            Type = type;
        }

        // Abstract method to be implemented by subclasses for displaying dashboard
        public abstract void DisplayDashboard();

        // Static method to create a new Dashboard
        public static Dashboard CreateDashboard(string name, DateTime requestedStartDate, DateTime requestedEndDate, int validityDuration, DashboardType type)
        {
            // You can choose which type of dashboard you want to create
            // For now, let's assume we are creating a ManufacturerDashboard as an example
            return new ManufacturerDashboard(name, requestedStartDate, requestedEndDate, validityDuration, type);
        }

        // Method to update an existing Dashboard
        public void UpdateDashboard(string name, DateTime requestedStartDate, DateTime requestedEndDate, int validityDuration)
        {
            Name = name;
            RequestedStartDate = requestedStartDate;
            RequestedEndDate = requestedEndDate;
            ValidityDuration = validityDuration;
        }

        
    }
}
