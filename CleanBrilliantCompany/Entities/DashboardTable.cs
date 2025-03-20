namespace CleanBrilliantCompany.Entities
{
    public class DashboardTable
    {
        public int DashboardId { get; set; }
        public string Name { get; set; }
        public DateTime RequestedStartDate { get; set; }
        public DateTime RequestedEndDate { get; set; }
        public DateTime GeneratedDate { get; set; }
        public int ValidityDuration { get; set; }
        public int TypeId { get; set; }
        public DashboardTypeTable Type { get; set; } // Navigation property

        public List<InventoryLevelTable> InventoryLevels { get; set; } = new List<InventoryLevelTable>(); // Added for the relationship
    }
}
