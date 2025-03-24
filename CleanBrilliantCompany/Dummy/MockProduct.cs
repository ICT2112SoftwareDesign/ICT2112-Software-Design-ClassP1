using CleanBrilliantCompany.Interface;

namespace CleanBrilliantCompany.Dummy
{
    public class MockProduct : IProduct
    {
        public Dictionary<int, int> GetProductStockLevels()
        {
            // Simulate product stock levels (product ID -> stock level)
            return new Dictionary<int, int>
            {
                //{ 1, 50 },  // Product 1: 50 units
                //{ 2, 200 }, // Product 2: 200 units
                //{ 3, 75 },  // Product 3: 75 units
                //{ 4, 150 }, // Product 4: 150 units
                //{ 5, 25 }   // Product 5: 25 units

                // Updated stock levels can be added here
                { 1, 57 },  // Product 1: 50 units
                { 2, 201 }, // Product 2: 200 units
                { 3, 72 },  // Product 3: 75 units
                { 4, 144 }, // Product 4: 150 units
                { 5, 19 },   // Product 5: 25 units
                { 6, 100 },   // Product 6: 100 units
                { 7, 2 },   // Product 7: 200 units
                { 8, 39 },   // Product 8: 300 units
                { 9, 180 },   // Product 9: 400 units
                { 10, 55 }   // Product 10: 500 units
            };
        }
    }
}
