using CleanBrilliantCompany.Interface;
using CleanBrilliantCompany.DatabaseEntities;

namespace CleanBrilliantCompany.Dummy
{
    public class MockProduct : Team6IProduct
    {
        private readonly SimulatedDbContext _context;

        public MockProduct(SimulatedDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public List<ProductTable> getAllProducts()
        {
            return _context.Products
                .Where(product => product.productId != 909) // Exclude Product 909
                .ToList();
        }
    }
}
