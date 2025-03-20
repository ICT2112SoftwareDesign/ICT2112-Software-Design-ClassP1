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
                { 1, 50 },  // Product 1: 50 units
                { 2, 200 }, // Product 2: 200 units
                { 3, 75 },  // Product 3: 75 units
                { 4, 150 }, // Product 4: 150 units
                { 5, 25 }   // Product 5: 25 units
            };
        }
    }
}
