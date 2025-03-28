namespace CleanBrilliantCompany.DTO
{
    public class InventoryDTO
    {
        // public int InventoryId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCategory { get; set; }
        public int StockLevel { get; set; }
        public int Threshold { get; set; }
        public string StockStatus { get; set; }
        public bool ReplenishmentStatus { get; set; }
        public int DashboardId { get; set; }
    }
}
