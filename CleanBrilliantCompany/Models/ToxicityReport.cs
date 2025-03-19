using System.Collections.Generic;
using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Models
{
    public class ToxicityReport
    {
        public string ProductName { get; set; }
        public List<IngredientSDM> Ingredients { get; set; }
        public float OverallToxicityScore { get; set; }
        public string ToxicityClassification { get; set; }
        public string SafetyRecommendation { get; set; }
    }
}
