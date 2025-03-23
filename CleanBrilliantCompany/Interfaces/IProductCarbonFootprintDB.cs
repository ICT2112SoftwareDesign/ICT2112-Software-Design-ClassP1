using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IProductCarbonFootprintDB
    {
        bool insertProductCF(int productId, string productName, string productCategory, double carbonEmission, string ecoStatus, DateTime dateCreated);
        double retrieveProductCarbonFootprint(int productCFId);
        List<ProductCarbonFootprintRDM> retrieveAllProductCarbonFootprint();
        float retrieveTotalCarbonFootprint();
        bool getQueryStatus();
    }
}
