namespace CleanBrilliantCompany.Interfaces
{
    public interface IChatbotQuery
    {
        string handleQuery(Int32 customerID, String query);
    }
}