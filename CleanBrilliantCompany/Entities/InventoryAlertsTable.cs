namespace CleanBrilliantCompany.Entities
{
    public class InventoryAlertsTable
    {
        public int AlertId { get; set; }
        public int InventoryId { get; set; }
        public InventoryLevelTable InventoryLevel { get; set; }
        public string AlertTypeCode { get; set; }
        public AlertTypeTable AlertType { get; set; }
        public DateTime AlertDate { get; set; }
    }
}
