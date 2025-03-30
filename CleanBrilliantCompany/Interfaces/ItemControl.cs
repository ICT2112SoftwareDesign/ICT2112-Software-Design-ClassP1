using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Models
{
    public class ItemControl : IOrderFulfilment
    {
        private readonly Random _random = new Random(); // Random generator for ItemIds

        // Adjust inventory levels and return item IDs for the order
        public List<Item> adjustInventory(int orderId, Dictionary<int, int> orderProducts)
        {
            List<Item> items = new List<Item>();

            foreach (var entry in orderProducts)
            {
                int productId = entry.Key;
                int quantity = entry.Value;

                // Generate random ItemIds for the given quantity
                for (int i = 0; i < quantity; i++)
                {
                    int itemId = _random.Next(1000, 9999); // Generate a random 4-digit ItemId
                    items.Add(new Item { itemId = itemId, productId = productId });
                }
            }

            return items;
        }

        // Process a cancelled order and restore inventory levels
        public void processCancelledOrder(int orderId)
        {
            // Simulate restoring inventory (hardcoded logic)
        }

        // Retrieve items that need to be returned
        public async Task<List<Item>> getToReturnItems()
        {
            // Simulate returning items (hardcoded logic)
            return await Task.FromResult(new List<Item>
            {
                new Item { itemId = 1234, productId = 101 },
                new Item { itemId = 5678, productId = 102 }
            });
        }
    }

    // Example Item class (assuming it exists in your project)
    public class Item
    {
        public int itemId { get; set; }
        public int productId { get; set; }
    }
}