namespace CleanBrilliantCompany.Entities
{
    public class InventoryLevelTable
    {
        public int InventoryId { get; set; }
        public int ProductId { get; set; }
        public int StockLevel { get; set; }
        public bool ReplenishmentStatus { get; set; }
        public int DashboardId { get; set; } // Foreign key to DashboardTable
    }
}
