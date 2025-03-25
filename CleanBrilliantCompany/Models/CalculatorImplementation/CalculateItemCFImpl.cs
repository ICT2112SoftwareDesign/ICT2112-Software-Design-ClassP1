using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Models.CalculatorImplementation
{
    public class CalculateItemCFImpl : IItemCFCalculator
    {
        private readonly IStorageDuration _storageDuration;
        private readonly IProductCFManagement _productCFManagement;
        private readonly IItemCFManagement _itemCFManagement;

        public CalculateItemCFImpl(IStorageDuration storageDuration, IProductCFManagement productCFManagement, IItemCFManagement itemCFManagement)
        {
            _storageDuration = storageDuration;
            _productCFManagement = productCFManagement;
            _itemCFManagement = itemCFManagement;
        }

        public bool CalculateCarbonFootprint(int itemId, int productId)
        {
            double baseCF = _productCFManagement.getProductCarbonFootprint(productId);
            double EXPONENT_CONSTANT = 1.02;
            double itemCFDouble = baseCF * Math.Pow(EXPONENT_CONSTANT, _storageDuration.GetStorageDuration(itemId));
            float itemCF = (float)itemCFDouble;
            string ecoStatus = itemCF >= 250 ? "Not Eco-Friendly" : "Eco-Friendly";

            try
            {
                // add new item CF entry into item CF record DB
                _itemCFManagement.addItemCF(
                    itemId,
                    productId,
                    itemCF,
                    ecoStatus,
                    DateTime.Now
                    );
            }
            catch(Exception e)
            {
                // Error caught
                return false;
            }

            return true;
        }
    }
}
