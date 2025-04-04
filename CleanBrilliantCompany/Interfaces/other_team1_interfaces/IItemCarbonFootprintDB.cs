using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IItemCarbonFootprintDB
    {
        bool insertItemCF(int itemId, int productId, double carbonEmission, string ecoStatus, DateTime dateCreated);
        bool updateAllItemCF();
        double retrieveItemCarbonFootprint(int itemCFId);
        double retrieveItemCarbonFootprintByItemId(int itemId);
        List<ItemCarbonFootprintRDM> retrieveItemCarbonFootprintByProductId(int itemProductId);
        List<ItemCarbonFootprintRDM> retrieveAllItemCarbonFootprint();
        float retrieveTotalCarbonFootprint();
        float retrieveTotalEcoFriendlyCarbonFootprint();
        bool getQueryStatus();
    }
}
