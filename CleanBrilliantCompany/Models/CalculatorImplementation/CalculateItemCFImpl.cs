using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Models.CalculatorImplementation
{
    public class CalculateItemCFImpl : IItemCFCalculator, IProduct
    {
        public bool CalculateCarbonFootprint(int itemId, int productId)
        {
            // route
            // itemDuration = retrieve storageduration in warehouse for item via IStorageDuration?
            // return item.weight * item.toxic * itemDuration
            return true;
        }
    }
}
