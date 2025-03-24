namespace CleanBrilliantCompany.Interfaces
{
    public interface IChatbotQuery
    {
        (string responseText, Dictionary<string, string> parameters) handleQuery(string query);
    }
}