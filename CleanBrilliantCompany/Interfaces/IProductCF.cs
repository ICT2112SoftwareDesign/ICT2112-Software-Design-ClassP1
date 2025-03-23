using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IProductCF
    {
        double getProductCarbonFootprint(int productCFId);
        List<ProductCarbonFootprintRDM> getAllProductCarbonFootprint();
        float getTotalCarbonFootprint();
    }
}
