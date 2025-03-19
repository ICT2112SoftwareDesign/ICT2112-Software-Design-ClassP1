using System.Collections.Generic;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Models.Entity;
using CleanBrilliantCompany.Models.Control;

namespace CleanBrilliantCompany.Models
{
    public class ToxicityReport
    {
        public string ProductName { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public float OverallToxicityScore { get; set; }
    }
}
