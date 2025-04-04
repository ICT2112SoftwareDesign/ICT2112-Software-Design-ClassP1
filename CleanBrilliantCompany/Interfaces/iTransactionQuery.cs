using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Interfaces
{
    public interface iTransactionQuery
    {

        List<Transaction> getAllTransactions(int pageNum, int pageSize);

        List<Transaction> getAllTransactions();

        int getTransactionCount();

        List<Transaction> getTransactionByItem(int itemId);

        List<Transaction> getTransactionByDateTime(DateTime dateTime);

    }
}