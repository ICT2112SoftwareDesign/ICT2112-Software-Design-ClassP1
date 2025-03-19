namespace CleanBrilliantCompany.Interfaces
{
    public interface IToxicity
    {
        Task<float> RetrieveToxicity(int ingredientId);
    }
}
