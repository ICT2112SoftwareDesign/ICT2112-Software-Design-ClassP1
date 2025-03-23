using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IItemCarbonFootprintDB
    {
        bool insertItemCF(int itemId, int productId, double carbonEmission, string ecoStatus, DateTime dateCreated);
        bool updateAllItemCF();
        double retrieveItemCarbonFootprint(int itemCFId);
        List<ItemCarbonFootprintRDM> retrieveAllItemCarbonFootprint();
        float retrieveTotalCarbonFootprint();
        bool getQueryStatus();
    }
}
