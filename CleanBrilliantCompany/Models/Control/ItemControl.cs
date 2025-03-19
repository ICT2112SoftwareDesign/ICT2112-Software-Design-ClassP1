using CleanBrilliantCompany.Models.Entity;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Mappers;
using System;
using System.Collections.Generic;

namespace CleanBrilliantCompany.Models.Control
{
    public class ItemControl : iItemQuery, iItemUpdate, iItem
    {
        private readonly ItemMapper _itemMapper;

        // Constructor that takes the connection string
        public ItemControl(string connectionString)
        {
            _itemMapper = new ItemMapper(connectionString);
            Console.WriteLine("Products loaded from database.");
        }

        // methods from iItemQuery
        public async Task<List<Item>> getAllItems()
        {
            return await Task.FromResult(_itemMapper.getAllItems()); // mapper uses iItemQuery to interact with control 
        }

        public async Task<Item> getItemById(int itemId)
        {
            return await Task.FromResult(_itemMapper.getItemById(itemId));
        }

        public async Task<bool> createItem(int itemId, int productId, float salePrice, int batchCode, int warehouseId, ItemStatus status)
        {
            return await Task.FromResult(_itemMapper.createItem(itemId, productId, salePrice, batchCode, warehouseId, status));
        }

        public async Task<bool> updateItem(int itemId, float salePrice)
        {
            return await Task.FromResult(_itemMapper.updateItem(itemId, salePrice));
        }

        public async Task<bool> updateItemStatus(int itemId, ItemStatus status)
        {
            return await Task.FromResult(_itemMapper.updateItemStatus(itemId, status));
        }
    }
}
