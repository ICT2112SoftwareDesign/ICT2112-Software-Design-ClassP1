using CleanBrilliantCompany.Mappers;

namespace CleanBrilliantCompany.Models.Factory
{
    public class SolidProductFactory : ProductFactory
    {
        private readonly ProductMapper _productMapper;
        public SolidProductFactory(ProductMapper productMapper) : base(productMapper)
        {
            _productMapper = productMapper;
        }

        public override int CreateProduct(string productName, string category, float productCost,
                                           int manufacturerId, float weight, int quantity, int volumeOrZero,
                                           float toxicityPercentage, int carbonFootprint, bool isLiquid)
        {
            // Since it is a solid product, volume is set to 0 and state to "0" (Solid)
            string productState = "0";
            int volume = 0;

            // Call the insert method from the mapper
            int productId = _productMapper.insert(productName, category, productCost,
                                                  manufacturerId, weight, quantity, volume,
                                                  toxicityPercentage, carbonFootprint, productState);

            // Return true if the product was successfully created (ID is positive)
            return productId;
        }
    }
}