// + getTransactionsByDateTime(dateTime): void

// + getTransactionByItem(itemId): void

// + getTransactions(): void

using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models.Entity;
using CleanBrilliantCompany.Models.Mapper;
// using CleanBrilliantCompany.Mappers;

namespace CleanBrilliantCompany.Models.Control{
    //public class TransactionControl : iTransactionQuery --> !Comment out first since i have not created the interface for iTQ
    public class TransactionControl : iTransactionQuery

    {

        private readonly TransactionMapper _transactionMapper;
        //For now, getTransactions are set to void returns as they are not used for further processing
        //If needed, will update return type to List<Transaction>
                // Constructor that takes the connection string
        public TransactionControl(string connectionString)
        {
            _transactionMapper = new TransactionMapper(connectionString);
            Console.WriteLine("Transactions loaded from database.");
        }

        public List<Transaction> getAllTransactions() {
            return _transactionMapper.getAllTransactions();
        }

        // public void GetTransactions()
        // {

        // }

        // public List<Transaction> GetTransactionsByItem(int itemId)
        // {
        //     return _transactions.FindAll(t => t.ItemId == itemId);
        // }

        // public void AddTransaction(Transaction transaction)
        // {
        //     _transactions.Add(transaction);
        // }

        // public List<Transaction> getTransactionsByDateTime(DateTime datetime)
        // {
        //     var transactions = _transactions.Where(t => t.DateTime.Date == datetime.Date).ToList();

        //     foreach (var transaction in transactions)
        //     {
        //         Console.WriteLine($"Transaction ID: {transaction.TransactionId}, Type: {transaction.AdjustmentType}, Date: {transaction.DateTime}");
        //     }    

        //     return transactions;
        // }

    }
}