using System;
using System.ComponentModel.DataAnnotations;

public class AgingAnalyticsDetailsTable
{
    [Key]
    public int AnalyticsId { get; set; }
    public int BatchCode { get; set; }  // Foreign Key linking to Batch (if applicable)
    public int DashboardId { get; set; } // Foreign Key linking to Dashboard
    public int ProductId { get; set; }  // Foreign Key linking to Product (if applicable)

    public int DaysInStorage { get; set; }
    public bool IsExpired { get; set; }
    public int RemainingDays { get; set; }
    public double TurnOverRate { get; set; }
    public double DeadStockPercentage { get; set; }

    // Optionally, navigation properties can be added if you want to link to related tables in the model
    // public DashboardTable Dashboard { get; set; }
    // public Product Product { get; set; }
}
