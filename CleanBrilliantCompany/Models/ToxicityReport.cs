using System.Collections.Generic;

namespace CleanBrilliantCompany.Models
{
    public class ToxicityReport
    {
        public string ProductName { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public float OverallToxicityScore { get; set; }
    }
}
