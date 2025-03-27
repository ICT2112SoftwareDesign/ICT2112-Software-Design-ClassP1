// + getTransactionsByDateTime(dateTime): void

// + getTransactionByItem(itemId): void

// + getTransactions(): void

using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models.Entity;
using CleanBrilliantCompany.Models.Mapper;

namespace CleanBrilliantCompany.Models.Control
{
    public class TransactionControl : iTransactionQuery, IObserver

    {

        private readonly TransactionMapper _transactionMapper;

        //When any changes to status are detected here --> the necessary code will run here 
        //TODO: Add record into database
        public void Update(Dictionary<string, object> itemInfo)
        {
            int itemId = Convert.ToInt32(itemInfo["ItemId"]);
            var itemStatus = itemInfo["ItemStatus"];
            int productId = Convert.ToInt32(itemInfo["ProductId"]);
            Console.WriteLine("ProductId: " + productId.GetType());
            var productName = itemInfo["ProductName"];
            var transactionDate = DateTime.Now;
            var staffId = 1;

            Console.WriteLine("Product Name Received in Update (TC): " + productName);

            var stringStatus = itemStatus.ToString();

            // Simple switch case based on itemStatus
            switch (itemStatus)
            {
                case ItemStatus.Available:
                    Console.WriteLine($"Item {itemId}: {productName} is {itemStatus}.");
                    break;

                case ItemStatus.Reserved:
                    Console.WriteLine($"Item {itemId}: {productName} is Reserved.");
                    stringStatus = "Reserved";
                    //Fields to pass in: transactionDate, adjustmentType, productId, itemId, staffId
                    _transactionMapper.createTransaction(transactionDate, stringStatus, productId, itemId, staffId);
                    break;

                case ItemStatus.Sold:
                    Console.WriteLine($"Item {itemId}: {productName} is Sold.");
                    stringStatus = "Sold";
                    _transactionMapper.createTransaction(transactionDate, stringStatus, productId, itemId, staffId);
                    break;

                case ItemStatus.Refunded:
                    Console.WriteLine($"Item {itemId}: {productName} is Refunded.");
                    stringStatus = "Refunded";
                    _transactionMapper.createTransaction(transactionDate, stringStatus, productId, itemId, staffId);
                    break;

                case ItemStatus.Transferred:
                    Console.WriteLine($"Item {itemId}: {productName} is Transferred.");
                    stringStatus = "Transferred";
                    _transactionMapper.createTransaction(transactionDate, stringStatus, productId, itemId, staffId);
                    break;

                case ItemStatus.Returned:
                    Console.WriteLine($"Item {itemId}: {productName} is Returned.");
                    stringStatus = "Returned";
                    _transactionMapper.createTransaction(transactionDate, stringStatus, productId, itemId, staffId);
                    break;      

                // case ItemStatus.ToReturn:
                //     Console.WriteLine($"Item {itemId}: {productName} is refunded and will be returned to the manufacturer.");
                //     stringStatus = "ToReturn";
                //     _transactionMapper.createTransaction(transactionDate, stringStatus, productId, itemId, staffId);
                //     break;         

                default:
                    Console.WriteLine($"Item {itemId}: {productName} status is unknown.");
                    break;
            }

        }



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

        public List<Transaction> getTransactionByDateTime(DateTime dateTime)
        {
            return _transactionMapper.getTransactionByDateTime(dateTime);
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