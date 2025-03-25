using CleanBrilliantCompany.Models.Entity;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Mappers;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace CleanBrilliantCompany.Models.Control
{
    public class ItemControl : IItemQuery, IItemUpdate, IItem, IReserve, IOrderFufilment, IRefundDetails, IItemCreation, IWarehouse
    {
        private readonly ItemMapper _itemMapper;

        private readonly TransactionControl _transactionObserver; //Added observer

        private readonly iProduct _iproductInterface;

        // Constructor that takes the connection string
        public ItemControl(IConfiguration configuration, iProduct iproductInterface)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found");
            Console.WriteLine(connectionString);
            _itemMapper = new ItemMapper(connectionString);
            _iproductInterface = iproductInterface;
            Console.WriteLine("Products loaded from database.");
            _transactionObserver = new TransactionControl(connectionString);
        }

        // METHODS FOR IITEM
        public async Task<List<Item>> getAllItems()
        {
            return await Task.FromResult(_itemMapper.getAllItems()); // mapper uses iItemQuery to interact with control 
        }

        public async Task<Item> getItemById(int itemId)
        {
            return await Task.FromResult(_itemMapper.getItemById(itemId));
        }

        // search by product name
        public async Task<List<Item>> getItemByProductName(string productName)
        {
            return await Task.FromResult(_itemMapper.getItemByProductName(productName));
        }

        public async Task<bool> createItem(int productId, float salePrice, int batchCode, int warehouseId, ItemStatus status)
        {
            return await Task.FromResult(_itemMapper.createItem(productId, salePrice, batchCode, warehouseId, status));
        }

        public async Task<bool> updateItem(int itemId, float salePrice)
        {
            return await Task.FromResult(_itemMapper.updateItem(itemId, salePrice));
        }

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
            //return await Task.FromResult(_itemMapper.updateItemStatus(itemId, reservationId, orderId, transferId, returnId, status));
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

        // for transaction feature, might remove in future
        public async Task<bool> updateItemStatusOld(int itemId, ItemStatus status)
        {
            return await Task.FromResult(_itemMapper.updateItemStatusOld(itemId, status));
        }


        // METHODS FOR RESERVE FEATURE (IRESERVE)
        public async Task<List<Item>> getItemsByStatus(ItemStatus itemStatus)
        {
            return await Task.FromResult(_itemMapper.getItemByStatus(itemStatus));
        }

        public Task<Product> retrieveProductDetails(int productId)
        {
            Product product = _iproductInterface.getProductDetails(productId);
            return Task.FromResult(product);
        }

        // METHODS FOR TRANSFER FEATURE (IWAREHOUSE)
        public async Task<Warehouse> getWarehouseDetails(int warehouseId)
        {
            return await Task.FromResult(_itemMapper.getWarehouseDetails(warehouseId));
        }

        public async Task<List<Item>> getItemByProductAndWarehouse(int warehouseId, int productId)
        {
            return await Task.FromResult(_itemMapper.getItemByProductAndWarehouse(productId, warehouseId));
        }

        public async Task<int> getProductQuantityByWarehouse(int productId, int warehouseId)
        {
            return await Task.FromResult(_itemMapper.getProductQuantityByWarehouse(productId, warehouseId));
        }

        // METHOD FOR HANDLING REFUNDED ITEMS 
        public void returnItemToInventory(List<int> itemId, string refundReason)
        {
            itemId = [4, 5, 6];
            refundReason = "Defect";
            _itemMapper.returnItemToInventory(itemId, refundReason);
        }

        // METHOD FOR HANDLING ORDERED ITEMS 
        public List<Item> adjustInventory(int orderId, Dictionary<int, int> orderProducts)
        {
            List<Item> items = _itemMapper.adjustInventory(orderId, orderProducts);

            RegisterObserversList(items); //attach observers before updating
            foreach (var i in items)
            {
                i.UpdateStatus(ItemStatus.Sold);
            }

            return items;
        }

        public void processCancelledOrder(int orderId)
        {
            _itemMapper.processCancelledOrder(orderId);
        }

    }
}
