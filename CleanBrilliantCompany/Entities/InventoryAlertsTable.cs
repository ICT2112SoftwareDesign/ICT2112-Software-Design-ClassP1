using Humanizer;

namespace CleanBrilliantCompany.Entities
{
    public class InventoryAlertsTable
    {
        public int AlertId { get; set; }
        public int InventoryId { get; set; }    // Foreign key to InventoryLevelTable
        public int ProductId { get; set; } // Navigation property
        //public InventoryLevelTable InventoryLevel { get; set; }
        public string AlertType { get; set; }   // Foreign key to AlertTypeTable
        //public AlertTypeTable AlertTypes { get; set; } // Navigation property
        public DateTime AlertDate { get; set; }
    }
}
