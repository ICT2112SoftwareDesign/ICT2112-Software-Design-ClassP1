using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Models.CalculatorImplementation
{
    public class CalculateProductCFImpl : IProductCFCalculator
    {
        public float CalculateCarbonFootprint(float vol, float tox, int productId)
        {
            float carbonFootprint = vol * tox/100;
            // add carbonFootprint and productId into carbonFootprintRecord DB
            return carbonFootprint;
        }
    }
}
