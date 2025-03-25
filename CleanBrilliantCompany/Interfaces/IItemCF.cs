using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IItemCF
    {
        double getItemCarbonFootprint(int itemCFId);
        double getItemCarbonFootprintByItemId(int itemId);
        List<ItemCarbonFootprintRDM> getAllItemCarbonFootprint();
        float getTotalCarbonFootprint();
    }
}
