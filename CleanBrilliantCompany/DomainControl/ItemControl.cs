using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Data;
using System;
using System.Collections.Generic;

namespace CleanBrilliantCompany.DomainControl
{
    public class ItemControl : IItemQuery, IItem
    {
        private readonly ItemMapper _itemMapper;

        private readonly IProduct _product;

        // Constructor that takes the connection string
        public ItemControl(ItemMapper itemMapper, IProduct product)
        {
            _itemMapper = itemMapper;
            _product = product;
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

        public Task<Product> retrieveProductDetails(int productId)
        {
            Product product = _product.getProductDetails(productId);
            return Task.FromResult(product);
        }
    }
}