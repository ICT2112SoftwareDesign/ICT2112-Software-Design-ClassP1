using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Models.Control
{
    public class ToxicityClassificationStrategy : IToxicityClassificationStrategy
    {
        // Dictionary to store known regulatory limits for common chemicals
        private readonly Dictionary<string, float> _regulatoryLimits = new Dictionary<string, float>(StringComparer.OrdinalIgnoreCase)
        {
            { "Sodium Lauryl Sulfate", 0.5f },
            { "Parabens", 0.3f },
            { "Ammonia", 0.4f },
            { "Formaldehyde", 0.2f },
            { "Phthalates", 0.3f },
            { "Triclosan", 0.3f },
            { "Chlorine", 0.6f },
            { "Alcohol", 0.7f },
            { "Fragrance", 0.4f }
        };

        public async Task<string> Classify(float toxicityScore)
        {
            // Using Task.FromResult to make this truly async
            return await Task.FromResult(toxicityScore switch
            {
                < 0.3f => "Low Toxicity",
                < 0.7f => "Moderate Toxicity",
                _ => "High Toxicity"
            });
        }

        public async Task<string> GetSafetyRecommendation(float toxicityScore)
        {
            // Using Task.FromResult to make this truly async
            return await Task.FromResult(toxicityScore switch
            {
                < 0.3f => "Safe for regular use. No special handling required.",
                < 0.7f => "Use with caution. Wear protective equipment when handling.",
                _ => "Hazardous material. Use only with proper safety equipment and in well-ventilated areas."
            });
        }
        
        public async Task<string> GetIngredientRecommendation(float toxicityScore, string ingredientName)
        {
            // Get base recommendation asynchronously
            string baseRecommendation = await GetSafetyRecommendation(toxicityScore);
            
            string additionalInfo = string.Empty;
            
            // Enhanced ingredient-specific recommendations with more detail
            if (ingredientName.Contains("Alcohol", StringComparison.OrdinalIgnoreCase))
            {
                additionalInfo = " This ingredient can cause skin dryness and irritation. It may also contribute to environmental pollution during manufacturing.";
            }
            else if (ingredientName.Contains("Acid", StringComparison.OrdinalIgnoreCase))
            {
                additionalInfo = " This acidic ingredient may cause irritation and can be corrosive at high concentrations. Consider using buffered formulations.";
            }
            else if (ingredientName.Contains("Sulfate", StringComparison.OrdinalIgnoreCase))
            {
                additionalInfo = " Sulfates can cause irritation in sensitive individuals and may deplete natural oils. They're also associated with aquatic toxicity when released into waterways.";
            }
            else if (ingredientName.Contains("Ammonia", StringComparison.OrdinalIgnoreCase))
            {
                additionalInfo = " Ammonia-based ingredients can cause respiratory issues, skin irritation, and contribute to nitrogen pollution. Use in well-ventilated areas and consider alternatives.";
            }
            else if (ingredientName.Contains("Fragrance", StringComparison.OrdinalIgnoreCase) || 
                     ingredientName.Contains("Perfume", StringComparison.OrdinalIgnoreCase))
            {
                additionalInfo = " Fragrances are common allergens and may cause skin sensitivities or respiratory issues. They often contain phthalates and other undisclosed chemicals.";
            }
            else if (ingredientName.Contains("Paraben", StringComparison.OrdinalIgnoreCase))
            {
                additionalInfo = " Parabens may disrupt hormone function and have been found in breast cancer tissues. Consider paraben-free alternatives.";
            }
            else if (ingredientName.Contains("Formaldehyde", StringComparison.OrdinalIgnoreCase))
            {
                additionalInfo = " Formaldehyde and formaldehyde-releasing preservatives are known carcinogens. Strict ventilation and protective measures are required.";
            }
            
            // Use Task.Delay to simulate an async operation for demo purposes
            await Task.Delay(1);
            
            return baseRecommendation + additionalInfo;
        }

        public async Task<Dictionary<string, string>> AnalyzeProductSafety(List<IngredientSDM> ingredients)
        {
            var analysis = new Dictionary<string, string>();
            
            // Enhanced safety analysis with more detailed insights
            var highToxicityIngredients = ingredients.Where(i => i.IngredientToxicity >= 0.7).ToList();
            if (highToxicityIngredients.Any())
            {
                analysis["HighToxicityWarning"] = $"This product contains {highToxicityIngredients.Count} high-toxicity ingredients: {string.Join(", ", highToxicityIngredients.Select(i => i.IngredientName))}.";
            }
            
            // Environmental impact analysis with more depth
            float avgToxicity = (float)ingredients.Average(i => i.IngredientToxicity);
            analysis["EnvironmentalImpact"] = avgToxicity switch
            {
                < 0.3f => "Low environmental impact. Product is likely biodegradable with minimal aquatic toxicity.",
                < 0.7f => "Moderate environmental impact. Some ingredients may persist in the environment or affect aquatic life.",
                _ => "High environmental impact. Contains ingredients that may bioaccumulate or cause long-term environmental harm."
            };
            
            // Regulatory compliance analysis
            var regulatoryExceedings = ingredients
                .Where(i => _regulatoryLimits.ContainsKey(i.IngredientName) && i.IngredientToxicity > _regulatoryLimits[i.IngredientName])
                .ToList();
                
            if (regulatoryExceedings.Any())
            {
                analysis["RegulatoryCompliance"] = $"Warning: {regulatoryExceedings.Count} ingredients exceed recommended regulatory limits.";
            }
            else
            {
                analysis["RegulatoryCompliance"] = "All ingredients appear to be within regulatory guidelines.";
            }
            
            // Skin sensitivity analysis
            var sensitizingIngredients = ingredients
                .Count(i => i.IngredientName.Contains("Fragrance", StringComparison.OrdinalIgnoreCase) || 
                           i.IngredientName.Contains("Alcohol", StringComparison.OrdinalIgnoreCase) ||
                           i.IngredientName.Contains("Acid", StringComparison.OrdinalIgnoreCase));
                           
            analysis["SkinSensitivity"] = sensitizingIngredients > 0 
                ? $"Contains {sensitizingIngredients} potentially sensitizing ingredients. May cause reactions in sensitive individuals."
                : "Low risk of skin sensitization.";
            
            // Safety for special populations with more detail
            analysis["SpecialPopulations"] = avgToxicity < 0.4f ? 
                "Generally safe for most users. Suitable for regular consumer use." : 
                "Use caution with children, during pregnancy, or for those with respiratory conditions or sensitive skin.";
            
            // Cumulative effect analysis - new
            if (ingredients.Count(i => i.IngredientToxicity > 0.5) >= 3)
            {
                analysis["CumulativeEffects"] = "Multiple moderately toxic ingredients may have amplified effects when combined.";
            }
            
            // Add a small delay to make this method truly async
            await Task.Delay(1);
            
            return analysis;
        }
        
        public async Task<List<string>> RecommendAlternatives(float toxicityScore, string ingredientName)
        {
            var alternatives = new List<string>();
            
            // Only recommend alternatives for high toxicity ingredients
            if (toxicityScore >= 0.7f)
            {
                if (ingredientName.Contains("Sodium Lauryl Sulfate", StringComparison.OrdinalIgnoreCase))
                {
                    alternatives.Add("Cocamidopropyl Betaine (Lower toxicity alternative, 60% less environmental impact)");
                    alternatives.Add("Sodium Cocoyl Isethionate (Gentle alternative derived from coconut oil)");
                    alternatives.Add("Decyl Glucoside (Plant-based surfactant with minimal irritation potential)");
                }
                else if (ingredientName.Contains("Parabens", StringComparison.OrdinalIgnoreCase))
                {
                    alternatives.Add("Phenoxyethanol (Lower toxicity preservative with less bioaccumulation)");
                    alternatives.Add("Sodium Benzoate (Natural preservative effective in acidic formulations)");
                    alternatives.Add("Radish Root Ferment Filtrate (Natural antimicrobial with minimal environmental impact)");
                }
                else if (ingredientName.Contains("Ammonia", StringComparison.OrdinalIgnoreCase))
                {
                    alternatives.Add("Sodium Carbonate (Safer cleaning agent with 70% reduced respiratory impact)");
                    alternatives.Add("Citric Acid (Natural cleaning alternative derived from citrus fruits)");
                    alternatives.Add("Sodium Bicarbonate (Gentle abrasive with minimal environmental footprint)");
                }
                else if (ingredientName.Contains("Fragrance", StringComparison.OrdinalIgnoreCase))
                {
                    alternatives.Add("Essential oils (Natural alternative, though still potential allergens)");
                    alternatives.Add("Fragrance-free formulation (Completely eliminates sensitization risk)");
                    alternatives.Add("Botanical extracts (Plant-based scenting with reduced allergen profile)");
                }
                else if (ingredientName.Contains("Formaldehyde", StringComparison.OrdinalIgnoreCase))
                {
                    alternatives.Add("Sodium Hydroxymethylglycinate (Lower toxicity preservative)");
                    alternatives.Add("Potassium Sorbate (Food-grade preservative with minimal health concerns)");
                    alternatives.Add("Ethylhexylglycerin (Modern preservative enhancer with better safety profile)");
                }
                else
                {
                    // Generic alternatives with more specific information
                    alternatives.Add("Consider plant-derived alternatives with similar functionality and lower environmental impact");
                    alternatives.Add("Look for ingredients with established safety data and third-party certifications");
                    alternatives.Add("Explore green chemistry alternatives developed specifically to replace problematic ingredients");
                }
            }
            
            // Add a small delay to make this method truly async
            await Task.Delay(1);
            
            return alternatives;
        }
    }
}