using CleanBrilliantCompany.Mappers;
namespace CleanBrilliantCompany.Models.Factory
{
    public abstract class ProductFactory
    {
        private readonly ProductMapper _productMapper;

        public ProductFactory(ProductMapper productMapper)
        {
            _productMapper = productMapper;
        }

        public abstract int CreateProduct(string productName, string category, float productCost,
                                              int manufacturerId, float weight, int quantity, int volume,
                                              float toxicityPercentage, int carbonFootprint, bool isLiquid);
    }
}