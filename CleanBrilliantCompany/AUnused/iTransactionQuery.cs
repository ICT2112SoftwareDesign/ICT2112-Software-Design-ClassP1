using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Interfaces
{
    public interface ITransactionQuery
    {
        List<Transaction> getAllTransactions(int pageNum, int pageSize);

 }
}