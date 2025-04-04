using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IProductQuery
    {
        public int createProduct(string productName, string category, float productCost,
                              int manufacturerId, float weight, int quantity, int volumeOrZero,
                              float toxicityPercentage, int carbonFootprint, bool isLiquid);
        Product getProductDetails(int productId);
        ProductManufacturer getManufacturerDetails(int manufacturerId);

    }
}