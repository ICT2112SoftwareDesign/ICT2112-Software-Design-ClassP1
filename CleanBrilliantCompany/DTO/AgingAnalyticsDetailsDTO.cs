public class AgingAnalyticsDetailsDTO
{
    public int AnalyticsId { get; set; }
    public int BatchCode { get; set; } // Foreign Key linking to batch
    public int DashboardId { get; set; } // Foreign Key linking to Dashboard

    public int ProductId { get; set; } 
    public int DaysInStorage { get; set; }
    public bool IsExpired { get; set; }
    public int RemainingDays { get; set; }
    public double TurnOverRate { get; set; }
    public double DeadStockPercentage { get; set; }
}
