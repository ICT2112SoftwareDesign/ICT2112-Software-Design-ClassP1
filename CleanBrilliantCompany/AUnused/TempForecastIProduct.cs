using CleanBrilliantCompany.ForecastManagement.DTO;
using System;
using System.Collections.Generic;

namespace CleanBrilliantCompany.Interfaces
{
    public class TempForecastIProduct
    {
        private readonly List<ProductDTO> productList = new List<ProductDTO>()
        {
            //new ProductDTO(2, "UltraClean Detergent", "Powerful liquid detergent for all fabrics."),
            //new ProductDTO(14, "Sparkle Window Cleaner", "Streak-free glass cleaner for home and office."),
            //new ProductDTO(23, "Fresh Scent Air Freshener", "Long-lasting air freshener with lavender scent."),
            //new ProductDTO(167, "GermShield Disinfectant Spray", "Kills 99.9% of germs on surfaces."),
            //new ProductDTO(176, "SuperScrub Sponge Set", "Pack of 6 durable scrubbing sponges."),
            //new ProductDTO(203, "ShinyFlo Floor Polish", "High-gloss finish polish for hardwood floors."),
            //new ProductDTO(228, "EcoBreeze Fabric Softener", "Eco-friendly softener with a fresh linen scent."),
            //new ProductDTO(241, "ToughGrip Rubber Gloves", "Heavy-duty gloves for cleaning and dishwashing.")
        };

        public List<ProductDTO> GetProductList()
        {
            return productList;
        }
    }
}
