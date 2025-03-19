namespace CleanBrilliantCompany.Models
{
    public class InventoryAlerts
    {
        public int AlertId { get; set; }
        public int InventoryId { get; set; }
        public InventoryLevel InventoryLevel { get; set; }
        public string AlertTypeCode { get; set; }
        public AlertType AlertType { get; set; }
        public DateTime AlertDate { get; set; }
    }
}
