using CleanBrilliantCompany.Data;
using CleanBrilliantCompany.Entities;
using CleanBrilliantCompany.Interface;

namespace CleanBrilliantCompany.Dummy
{
    public class MockProduct : IProduct
    {
        private readonly AppDbContext _context;

        public MockProduct(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public List<ProductTable> getAllProducts()
        {
            return _context.ProductTable
                .Where(product => product.productId != 909) // Exclude Product 909
                .ToList();
        }
    }
}
