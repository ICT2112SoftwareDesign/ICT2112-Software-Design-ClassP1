using CleanBrilliantCompany.Models.Entity;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Mappers;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace CleanBrilliantCompany.Models.Control
{
    public class ItemControl : IItemQuery, IItemUpdate, IItem, IReserve, IOrderFufilment, IRefundDetails, IItemCreation, IWarehouse, IReturnForm, IItemDetails
    {
        private readonly ItemMapper _itemMapper;
        private readonly TransactionControl _transactionObserver; // Added observer
        private readonly IProductQuantity _iproductquantityInterface;

        // Constructor that takes the connection string
        public ItemControl(IConfiguration configuration, IProduct iproductInterface, IProductQuantity iproductquantityInterface)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found");
            _itemMapper = new ItemMapper(connectionString);
            _iproductquantityInterface = iproductquantityInterface;
            Console.WriteLine("Items loaded from database.");
            _transactionObserver = new TransactionControl(connectionString);
        }

        // get all items to display on ui
        public async Task<List<Item>> getAllItems(int pageNumber, int pageSize)
        {
            return await Task.FromResult(_itemMapper.getAllItems(pageNumber, pageSize)); // mapper uses iItemQuery to interact with control 
        }

        // get total item count (for pagination)
        public int getItemCount()
        {
            return _itemMapper.getItemCount();
        }

        // method for IItemDetails for team 6
        public async Task<List<Item>> getItems()
        {
            return await Task.FromResult(_itemMapper.getItems());
        }

        // search by item id & method used for IItem & IItemDetails
        public async Task<Item> getItemById(int itemId)
        {
            return await Task.FromResult(_itemMapper.getItemById(itemId));
        }

        // search by product name
        public async Task<List<Item>> getItemByProductName(string productName)
        {
            return await Task.FromResult(_itemMapper.getItemByProductName(productName));
        }

        // method for IItemCreation 
        public async Task<bool> createItem(int productId, int batchCode, int warehouseId, ItemStatus status)
        {
            Product product = await retrieveProductDetails(productId);
            List<Dictionary<string, object>> productInfo = new List<Dictionary<string, object>>();
            float salePrice = 0.0f;
            status = ItemStatus.Available;

            if (product != null)
            {
                productInfo.Add(product.retrieveProductInfo());

                float costPrice = Convert.ToSingle(productInfo[0]["ProductCost"]); // Safe conversion

                Console.WriteLine("==================");
                Console.WriteLine($"COST PRICE: {costPrice}");
                Console.WriteLine("==================");

                salePrice = MathF.Ceiling(costPrice * 1.3f * 10) / 10f;
            }


            return await Task.FromResult(_itemMapper.createItem(productId, salePrice, batchCode, warehouseId, status));
        }

        // update item sale price
        public async Task<bool> updateItem(int itemId, float salePrice)
        {
            return await Task.FromResult(_itemMapper.updateItem(itemId, salePrice));
        }

        // observer for transaction logs 
        public void RegisterObservers(Item item)
        {
            item.Attach(_transactionObserver);
            Console.WriteLine("Called registerObservers method and attached observer to item");
        }

        public void RegisterObserversList(List<Item> item)
        {
            foreach (var i in item)
            {
                i.Attach(_transactionObserver);
                Console.WriteLine("Called registerObservers method and attached observer to item");
            }

        }

        // METHOD FOR IITEMUPDATE 
        public async Task<bool> updateItemStatus(int itemId, int? reservationId, int? orderId, int? transferId, int? returnId, ItemStatus status)
        {
            bool dbUpdated = await Task.FromResult(_itemMapper.updateItemStatus(itemId, reservationId, orderId, transferId, returnId, status));

            if (dbUpdated)
            {
                Item item = await getItemById(itemId);

                if (item != null)
                {
                    Console.WriteLine("CALLING UPDATE ITEM STATUS IN CONTROL");
                    RegisterObservers(item); //attach observers before updating
                    item.UpdateStatus(status); //update and notify observers
                }
            }

            return dbUpdated;
        }

        // METHOD FOR IReserve
        public async Task<List<Item>> getItemsByStatus(ItemStatus itemStatus)
        {
            return await Task.FromResult(_itemMapper.getItemByStatus(itemStatus));
        }

        // using from IProductQuantity & implementing inside IReserve & IReturnForm
        public Task<Product> retrieveProductDetails(int productId)
        {
            Product product = _iproductquantityInterface.getProductDetails(productId);
            return Task.FromResult(product);
        }

        // using from IProductQuantity & implementing inside IItemUpdate
        public void updateProductQuantity(int productId, int quantity, string arithmeticOperations)
        {
            _iproductquantityInterface.updateQuantity(productId, quantity, arithmeticOperations);
        }


        // METHOD FOR HANDLING REFUNDED ITEMS (Module 1 IRefundDetails)
        public void returnItemToInventory(List<int> itemId, string refundReason)
        {
            itemId = [4, 5, 6];
            refundReason = "Defect";
            _itemMapper.returnItemToInventory(itemId, refundReason);
        }

        // METHOD FOR HANDLING ORDERED ITEMS (Module 1 IOrderFufilment)
        public List<Item> adjustInventory(int orderId, Dictionary<int, int> orderProducts)
        {
            List<Item> items = _itemMapper.adjustInventory(orderId, orderProducts);

            if (items != null)
            {
                foreach (var entry in orderProducts)
                {
                    int productId = entry.Key;
                    int quantity = entry.Value;
                    string arithmeticOperations = "decrease";

                    updateProductQuantity(productId, quantity, arithmeticOperations);
                }
            }

            RegisterObserversList(items); //attach observers before updating
            foreach (var i in items)
            {
                i.UpdateStatus(ItemStatus.Sold);
            }

            return items;
        }

        // METHOD FOR HANDLING CANCELLED ORDERS (Module 1 IOrderFufilment)
        public void processCancelledOrder(int orderId)
        {
            updateProductQuantity(2, 2, "increase");
            _itemMapper.processCancelledOrder(orderId);
        }

        public async Task<List<Item>> getToReturnItems()
        {
            return await Task.FromResult(_itemMapper.getToReturnItems());
        }
        
        // METHODS FOR TRANSFER FEATURE (IWAREHOUSE)
        public async Task<Warehouse> getWarehouseDetails(int warehouseId)
        {
            return await Task.FromResult(_itemMapper.getWarehouseDetails(warehouseId));
        }

        public async Task<List<Item>> getItemByProductAndWarehouse(int productId, int quantity, int warehouseId)
        {
            return await Task.FromResult(_itemMapper.getItemByProductAndWarehouse(productId, quantity, warehouseId));
        }

        public async Task<int> getProductQuantityByWarehouse(int productId, int warehouseId)
        {
            return await Task.FromResult(_itemMapper.getProductQuantityByWarehouse(productId, warehouseId));
        }

        public async Task<List<Item>> getTransferredItems(int transferId)
        {
            return await Task.FromResult(_itemMapper.getTransferredItems(transferId));
        }

        public async Task<List<Product>> getLowStockProductInWarehouse()
        {
            return await Task.FromResult(_itemMapper.getLowStockProductInWarehouse());
        }


    }
}