using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanBrilliantCompany.DatabaseEntities
{
    public class ProductThresholdTable
    {
        [Key] // Primary Key
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public int? Threshold { get; set; }
        public DateTime? LastUpdated { get; set; }
    }
}
