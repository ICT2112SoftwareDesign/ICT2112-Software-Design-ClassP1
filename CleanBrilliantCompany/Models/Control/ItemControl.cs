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

        // Constructor that takes the connection string
        public ItemControl(string connectionString)
        {
            _itemMapper = new ItemMapper(connectionString);
            Console.WriteLine("Products loaded from database.");
        }

        // methods from iItemQuery
        public List<Item> getAllItems()
        {
            return _itemMapper.getAllItems();
        }

        public Item getItem(int itemId)
        {
            return _itemMapper.getItem(itemId);
        }
    }
}
