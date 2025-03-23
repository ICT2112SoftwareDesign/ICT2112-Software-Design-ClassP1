using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IItemCF
    {
        double getItemCarbonFootprint(int itemCFId);
        List<ItemCarbonFootprintRDM> getAllItemCarbonFootprint();
        float getTotalCarbonFootprint();
    }
}
