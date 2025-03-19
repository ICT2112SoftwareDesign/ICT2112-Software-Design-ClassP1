namespace CleanBrilliantCompany.Models
{
    public class InventoryLevel
    {
        public int InventoryId { get; set; }
        public int ProductId { get; set; }
        public int StockLevel { get; set; }
        public int Threshold { get; set; }
        public string StockCode { get; set; }
        public StockStatus StockStatus { get; set; }
        public bool ReplenishmentStatus { get; set; }
        public int DashboardId { get; set; }
        //public InventoryDashboard Dashboard { get; set; }
        public List<AlertType> AlertType { get; set; } = new List<AlertType>();
    }
}
