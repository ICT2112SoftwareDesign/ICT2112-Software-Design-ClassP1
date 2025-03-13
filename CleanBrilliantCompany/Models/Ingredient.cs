using System.ComponentModel.DataAnnotations.Schema;

namespace CleanBrilliantCompany.Models
{
    [Table("Ingredient")]
    public class Ingredient
    {
        public int IngredientId { get; set; }
        public int ProductId { get; set; }
        public string IngredientName { get; set; }
        public float IngredientToxicity { get; set; }
        public int ThresholdQuantity { get; set; }
        public string MeasurementUnit { get; set; }
        public bool ReorderStatus { get; set; }
    }
}
