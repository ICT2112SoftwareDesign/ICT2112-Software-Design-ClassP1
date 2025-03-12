using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Models.CalculatorImplementation
{
    public class CalculateProductCFImpl
    {
        public float CalculateCarbonFootprint(int productId, float vol, float tox)
        {
            float carbonFootprint = vol * tox/100;
            // add carbonFootprint and productId into carbonFootprintRecord DB
            return carbonFootprint;
        }
    }
}
