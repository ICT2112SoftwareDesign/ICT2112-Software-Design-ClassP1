using System.ComponentModel.DataAnnotations;

namespace CleanBrilliantCompany.DTO
{
    public class InventoryDTO
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductCategory { get; set; }

        public int StockLevel { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Threshold must be at least 1")]
        public int Threshold { get; set; }

        public DateTime? LastUpdated { get; set; }

        //public string StockStatus { get; set; }

        public bool ReplenishmentStatus { get; set; }

        public int DashboardId { get; set; }
    }
}
