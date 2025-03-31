using CleanBrilliantCompany.DatabaseEntities;

namespace CleanBrilliantCompany.Interface

{
    public interface Team6IProduct
    {
        // Returns a dictionary of product IDs and their stock levels
        //Dictionary<int, int> GetProductStockLevels();

        // Returns products table
        List<ProductTable> getAllProducts();
    }
}
