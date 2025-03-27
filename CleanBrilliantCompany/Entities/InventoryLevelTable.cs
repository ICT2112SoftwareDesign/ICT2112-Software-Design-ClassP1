namespace CleanBrilliantCompany.Entities
{
    public class InventoryLevelTable
    {
        public int InventoryId { get; set; }
        public int ProductId { get; set; }
        public int StockLevel { get; set; }
        public int Threshold { get; set; }
        public string StockCode { get; set; } // Foreign key to StockStatusTable
        //public StockStatusTable StockStatus { get; set; }
        public bool ReplenishmentStatus { get; set; }
        public int DashboardId { get; set; } // Foreign key to DashboardTable
        //public DashboardTable DashboardTable { get; set; } // Navigation property
        //public List<InventoryAlertsTable> AlertTypes { get; set; } = new List<InventoryAlertsTable>();
    }
}
