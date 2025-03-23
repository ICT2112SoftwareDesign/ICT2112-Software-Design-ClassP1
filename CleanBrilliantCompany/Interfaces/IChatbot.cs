namespace CleanBrilliantCompany.Interfaces
{
    public interface IChatbot
    {
        (string responseText, Dictionary<string, string> parameters) submitQuery(string query);
    }
}