using CleanBrilliantCompany.Models.Entity;
using CleanBrilliantCompany.Mappers;
namespace CleanBrilliantCompany.Models.Factory
{
    public class LiquidProductFactory : ProductFactory
    {
        private readonly ProductMapper _productMapper;
        
        public LiquidProductFactory(ProductMapper productMapper) : base(productMapper)
        {
            _productMapper = productMapper;
        }

        public override int CreateProduct(string productName, string category, float productCost,
                                            int manufacturerId, float weight, int quantity, int volumeOrZero,
                                            float toxicityPercentage, int carbonFootprint, bool isLiquid)
        {
            string productState = "1";  // 1 represents Liquid
            int volume = volumeOrZero;  // Volume should be set as provided
            int productId = _productMapper.insert(productName, category, productCost,
                                          manufacturerId, weight, quantity, volume,
                                          toxicityPercentage, carbonFootprint, productState);

             // Return true if the product was successfully created (ID is positive)
            return productId;
        }
    }
}