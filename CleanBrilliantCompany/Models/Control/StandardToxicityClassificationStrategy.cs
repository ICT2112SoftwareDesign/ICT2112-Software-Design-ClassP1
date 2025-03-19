using System.Threading.Tasks;
using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Models.Control
{
    public class StandardToxicityClassificationStrategy : IToxicityClassificationStrategy
    {
        public async Task<string> Classify(float toxicityScore)
        {
            return await Task.FromResult(toxicityScore switch
            {
                < 0.3f => "Low Toxicity",
                < 0.7f => "Moderate Toxicity",
                _ => "High Toxicity"
            });
        }

        public async Task<string> GetSafetyRecommendation(float toxicityScore)
        {
            return await Task.FromResult(toxicityScore switch
            {
                < 0.3f => "Safe for regular use. No special handling required.",
                < 0.7f => "Use with caution. Wear protective equipment when handling.",
                _ => "Hazardous material. Use only with proper safety equipment and in well-ventilated areas."
            });
        }
    }
}
