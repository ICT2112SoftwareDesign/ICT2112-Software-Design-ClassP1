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

        //Define IItem here
        private readonly IItem _iItem;


        //When any changes to status are detected here --> the necessary code will run here 
        public void Update(Dictionary<string, object> itemInfo)
        {
            int itemId = Convert.ToInt32(itemInfo["ItemId"]);
            var itemStatus = itemInfo["ItemStatus"];
            int productId = Convert.ToInt32(itemInfo["ProductId"]);
            var productName = itemInfo["ProductName"];
            var transactionDate = DateTime.Now;
            var staffId = 1;

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

                default:
                    Console.WriteLine($"Item {itemId}: {productName} status is unknown.");
                    break;
            }

        }

        // Constructor that takes the connection string, includes IItem in here to allow filtering of transactions by itemId
        public TransactionControl(string connectionString, IItem iItem)
        {
            _transactionMapper = new TransactionMapper(connectionString);
            _iItem = iItem;
            Console.WriteLine("Transactions loaded from database.");
        }

        public TransactionControl(string connectionString)
        {
            _transactionMapper = new TransactionMapper(connectionString);
            Console.WriteLine("Transactions loaded from database.");
        }

        //This method will replace getTransactions() in class diagram, used for getting all transactions + pagination
        public List<Transaction> getAllTransactions(int pageNumber, int pageSize)
        {
            return _transactionMapper.getAllTransactions(pageNumber, pageSize);
        }

        //Default getAllTransactions 
        public List<Transaction> getAllTransactions()
        {
            return _transactionMapper.getAllTransactions();
        }

        //Filter transactions by search date
        public List<Transaction> getTransactionByDateTime(DateTime dateTime)
        {
            return _transactionMapper.getTransactionByDateTime(dateTime);
        }

        //Used for pagination purposes
        public int getTransactionCount() {
            return _transactionMapper.getTransactionCount();
        }

        //For this method --> it will allow us to get all transactions done for a specific item --> it will return a list of all relevant transactions
        //Filter by item 
        public List<Transaction> getTransactionByItem(int itemId) {
            Item item = _iItem.getItemById(itemId).Result; // Blocking for simplicity, consider using async if needed

            if (item != null)
            {
                // You now have access to productId, itemId, etc.
                Console.WriteLine("Retrieved item using IItem interface:");
                //Console.WriteLine($"ItemId: {item}, ProductId: {item.ProductId}");

                // You can pass just the itemId to the mapper
                return _transactionMapper.getTransactionByItem(itemId);
            }
            else
            {
                Console.WriteLine($"No item found with ID {itemId}");
                return new List<Transaction>(); // return empty list if item not found
            }
        }

    }
}