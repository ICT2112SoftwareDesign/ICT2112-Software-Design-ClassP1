using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IProductCF
    {
        double getProductCarbonFootprint(int productId);
        List<ProductCarbonFootprintRDM> getAllProductCarbonFootprint();
        float getTotalCarbonFootprint();
    }
}
