namespace CleanBrilliantCompany.Interfaces
{
    public interface IProductCFCalculator
    {
        float CalculateCarbonFootprint(float volume, float toxicPercent, int productId);
    }
}
