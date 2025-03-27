using CleanBrilliantCompany.Data;
using CleanBrilliantCompany.Interface;

namespace CleanBrilliantCompany.Dummy
{
    public class MockProduct : IProduct
    {
        //public Dictionary<int, int> GetProductStockLevels()
        //{
        //    // Simulate product stock levels (product ID -> stock level)
        //    return new Dictionary<int, int>
        //    {
        //        //{ 1, 50 },  // Product 1: 50 units
        //        //{ 2, 200 }, // Product 2: 200 units
        //        //{ 3, 75 },  // Product 3: 75 units
        //        //{ 4, 150 }, // Product 4: 150 units
        //        //{ 5, 25 }   // Product 5: 25 units

        //        // Updated stock levels can be added here
        //        { 1, 57 },
        //        { 2, 201 },
        //        { 3, 72 },
        //        { 4, 144 },
        //        { 5, 19 },
        //        { 6, 100 },
        //        { 7, 2 },
        //        { 8, 39 },
        //        { 9, 180 },
        //        { 10, 55 }
        //    };
        //}

        private readonly AppDbContext _context;

        public MockProduct(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Dictionary<int, int> GetProductStockLevels()
        {
            // Fetch products from the database and map productId to quantity
            return _context.ProductTable
                .Where(product => product.productId != 909) // Exclude Product 909
                .ToDictionary(
                    product => product.productId,
                    product => product.quantity
                );
        }
    }
}
