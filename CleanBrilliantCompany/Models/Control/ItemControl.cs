using CleanBrilliantCompany.Models.Entity;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Mappers;
using System;
using System.Collections.Generic;

namespace CleanBrilliantCompany.Models.Control
{
    public class ItemControl : iItemQuery
    {
        private readonly ItemMapper _itemMapper;
        private readonly TransactionControl _transactionObserver; // Added observer

        // Constructor that takes the connection string
        public ItemControl(string connectionString)
        {
            _itemMapper = new ItemMapper(connectionString);
            Console.WriteLine("Products loaded from database.");
            _transactionObserver = new TransactionControl(connectionString);
        }

        // methods from iItemQuery
        public async Task<List<Item>> getAllItems()
        {
            return await Task.FromResult(_itemMapper.getAllItems()); // mapper uses iItemQuery to interact with control 
        }

        public async Task<Item> getItem(int itemId)
        {
            return await Task.FromResult(_itemMapper.getItem(itemId));
        }

        public async Task<bool> createItem(int itemId, int productId, float salePrice, int batchCode, int warehouseId, ItemStatus status)
        {
            return await Task.FromResult(_itemMapper.createItem(itemId, productId, salePrice, batchCode, warehouseId, status));
        }

        public async Task<bool> updateItem(int itemId, float salePrice)
        {
            return await Task.FromResult(_itemMapper.updateItem(itemId, salePrice));
        }

        public void RegisterObservers(Item item)
        {
            Console.WriteLine("Called registerObservers method");
            item.Attach(_transactionObserver);
            Console.WriteLine("Attached transactionObserver to item");
        }
        public async Task<bool> updateItemStatus(int itemId, ItemStatus status)
        {
            bool dbUpdated = await Task.FromResult(_itemMapper.updateItemStatus(itemId, status)); //Keep _itemMapper call

            if (dbUpdated)
            {
                Item item = await getItem(itemId); //Fetch the Item object
                // Console.WriteLine("Item in IC.cs" + item.retrieveItemInfo());
                // foreach (var kvp in item.retrieveItemInfo())
                // {
                //     Console.WriteLine($"{kvp.Key}: {kvp.Value}");
                // }

                if (item != null)
                {
                    RegisterObservers(item); //Attach observers before updating
                    //Console.WriteLine("Executed line 77 of IC.cs");
                    item.UpdateStatus(status); //Update & notify observers
                    //Console.WriteLine("Executed line 79 of IC.cs");
                }
            }

            return dbUpdated;
        }

    }
}
