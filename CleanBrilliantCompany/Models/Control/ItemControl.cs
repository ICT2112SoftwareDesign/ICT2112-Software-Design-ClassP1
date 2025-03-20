using CleanBrilliantCompany.Models.Entity;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Mappers;
using System;
using System.Collections.Generic;

namespace CleanBrilliantCompany.Models.Control
{
    public class ItemControl : IItemQuery, IItemUpdate, IItem, IReserve
    {
        private readonly ItemMapper _itemMapper;

        private readonly iProduct _iproductInterface;

        // Constructor that takes the connection string
        public ItemControl(string connectionString, iProduct iproductInterface)
        {
            _itemMapper = new ItemMapper(connectionString);
            _iproductInterface = iproductInterface;
            Console.WriteLine("Products loaded from database.");
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

        public async Task<bool> createItem(int itemId, int productId, float salePrice, int batchCode, int warehouseId, ItemStatus status)
        {
            return await Task.FromResult(_itemMapper.createItem(itemId, productId, salePrice, batchCode, warehouseId, status));
        }

        public async Task<bool> updateItem(int itemId, float salePrice)
        {
            return await Task.FromResult(_itemMapper.updateItem(itemId, salePrice));
        }

        // for transaction feature, might remove in future
        public async Task<bool> updateItemStatus(int itemId, ItemStatus status)
        {
            return await Task.FromResult(_itemMapper.updateItemStatus(itemId, status));
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
        public async Task<Warehouse> getWarehouseDetails(int warehouseId) {
            return await Task.FromResult(_itemMapper.getWarehouseDetails(warehouseId));
        }

        public async Task<List<Item>> getItemByProductAndWarehouse(int warehouseId, int productId) {
            return await Task.FromResult(_itemMapper.getItemByProductAndWarehouse(productId, warehouseId));
        }

        public async Task<int> getProductQuantityByWarehouse(int productId, int warehouseId) {
            return await Task.FromResult(_itemMapper.getProductQuantityByWarehouse(productId, warehouseId));
        }


    }
}
