using System.Collections.Generic;
using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.DTO
{
    public class ToxicityReport
    {
        // Initialize with default values to avoid null warnings
        public string ProductName { get; set; } = string.Empty;
        public List<IngredientSDM> Ingredients { get; set; } = new List<IngredientSDM>();
        public float OverallToxicityScore { get; set; }
        public string ToxicityClassification { get; set; } = string.Empty;
        public string SafetyRecommendation { get; set; } = string.Empty;
        
        // Enhanced analysis properties
        public Dictionary<string, string> ProductSafetyAnalysis { get; set; } = new Dictionary<string, string>();
        public Dictionary<int, List<string>> IngredientAlternatives { get; set; } = new Dictionary<int, List<string>>();
        public Dictionary<int, string> IngredientSpecificRecommendations { get; set; } = new Dictionary<int, string>();
        
        // New advanced analysis properties
        public Dictionary<string, string> ToxicityTrends { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> CorrelationAnalysis { get; set; } = new Dictionary<string, string>();
        
        // Methods to help with analysis visualization
        public string GetToxicityColor()
        {
            return ToxicityClassification.Contains("High") ? "danger" :
                   ToxicityClassification.Contains("Moderate") ? "warning" : "success";
        }
        
        public int GetHighToxicityCount()
        {
            return Ingredients?.Count(i => i.IngredientToxicity >= 0.7) ?? 0;
        }
        
        public int GetModerateToxicityCount()
        {
            return Ingredients?.Count(i => i.IngredientToxicity >= 0.3 && i.IngredientToxicity < 0.7) ?? 0;
        }
        
        public int GetLowToxicityCount()
        {
            return Ingredients?.Count(i => i.IngredientToxicity < 0.3) ?? 0;
        }
    }
}