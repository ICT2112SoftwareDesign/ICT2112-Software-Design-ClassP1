namespace CleanBrilliantCompany.Interfaces
{
    public interface IToxicityClassificationStrategy
    {
        Task<string> Classify(float toxicityScore);
        Task<string> GetSafetyRecommendation(float toxicityScore);
    }
}
