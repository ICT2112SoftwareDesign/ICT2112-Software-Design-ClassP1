namespace CleanBrilliantCompany.Entities
{
    public class InventoryAlertsTable
    {
        public int AlertId { get; set; }
        public int InventoryId { get; set; }
        public InventoryLevelTable InventoryLevel { get; set; }
        public string AlertType { get; set; }
        public AlertTypeTable AlertTypes { get; set; } // Navigation property
        public DateTime AlertDate { get; set; }
    }
}
