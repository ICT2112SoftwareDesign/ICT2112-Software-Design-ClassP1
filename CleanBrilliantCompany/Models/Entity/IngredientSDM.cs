using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanBrilliantCompany.Models.Entity
{
    [Table("Ingredient")]
    public class IngredientSDM
    {
        public int IngredientId { get; set; }
        public int ProductId { get; set; }
        public string IngredientName { get; set; }
        public double IngredientToxicity { get; set; }
        public int ThresholdQuantity { get; set; }
        public string MeasurementUnit { get; set; }
        public bool ReorderStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
