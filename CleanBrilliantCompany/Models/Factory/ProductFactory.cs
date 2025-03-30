using CleanBrilliantCompany.Mappers;
namespace CleanBrilliantCompany.Models.Factory
{
    public class ProductFactory
    {
        private readonly ProductMapper _productMapper;

        public ProductFactory(ProductMapper productMapper)
        {
            _productMapper = productMapper;
        }

        public void CreateProduct(string productName, string category, float productCost,
                                int manufacturerId, float weight, int quantity, int volumeOrZero, 
                                float toxicityPercentage, int carbonFootprint, bool isLiquid)
        {
            string productState = isLiquid ? "1" : "0";
            int volume = isLiquid ? volumeOrZero : 0;

            _productMapper.insert(productName, category, productCost,
                                manufacturerId, weight, quantity, volume,
                                toxicityPercentage, carbonFootprint, productState);
        }
    }
}
