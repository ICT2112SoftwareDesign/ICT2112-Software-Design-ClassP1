// + getTransactionsByDateTime(dateTime): void

// + getTransactionByItem(itemId): void

// + getTransactions(): void

using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models.Entity;
using CleanBrilliantCompany.Models.Mapper;

namespace CleanBrilliantCompany.Models.Control
{
    //public class TransactionControl : iTransactionQuery --> !Comment out first since i have not created the interface for iTQ
    public class TransactionControl : iTransactionQuery, IObserver

    {

        //When any changes to status are detected here --> the necessary code will run here 
        //TODO: Add record into database
        public void Update(Dictionary<string, object> itemInfo)
        {
            var itemId = itemInfo["ItemId"];
            var itemStatus = itemInfo["ItemStatus"];
            var productName = itemInfo["ProductName"];
            Console.WriteLine("ab" + productName);


            Console.WriteLine($"[Notification] Sending alert: Item Status Changed");
            // Console.WriteLine($"Item type: {itemId}");
            // Console.WriteLine($"Item status: {itemStatus}");
            // Console.WriteLine($"Product name: {productName}");

            foreach (var kvp in itemInfo)
            {
                Console.WriteLine($"{kvp.Key}: {kvp.Value}");
            }


        }

        private readonly TransactionMapper _transactionMapper;
        //For now, getTransactions are set to void returns as they are not used for further processing
        //If needed, will update return type to List<Transaction>
        // Constructor that takes the connection string
        public TransactionControl(string connectionString)
        {
            _transactionMapper = new TransactionMapper(connectionString);
            Console.WriteLine("Transactions loaded from database.");
        }

        //This method will replace getTransactions() in class diagram --> update afterwards
        public List<Transaction> getAllTransactions()
        {
            return _transactionMapper.getAllTransactions();
        }

        //Missing Method 1: getTransactionByItem(itemId): List<Transaction>
        //For this method --> it will allow us to get all transactions done for a specific item --> it will return a list of all relevant transactions
        //Flow for this will be user clicks a btn get specific item transactions --> they see a list of all items and products --> they select the item
        //get the selected itemId --> pass it in into the method here
        //Can probably reference how xj did for this --> where she pass the itemId

        // public List<Transaction> getTransactionByItem(itemId) {
        //     return _transactionMapper.getTransactionByItem();
        // }

        //Missing Method 2: getTransactionByDateTime(dateTime): List<Transaction>
        //Similarly --> pass in the dateTime var --> return a list of all transactions for that day itself



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