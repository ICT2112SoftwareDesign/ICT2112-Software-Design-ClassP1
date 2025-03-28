using CleanBrilliantCompany.Entities;
using System.Collections.Generic;

namespace CleanBrilliantCompany.Interface
{
    public interface IProduct
    {
        // Returns a dictionary of product IDs and their stock levels
        //Dictionary<int, int> GetProductStockLevels();

        // Returns products table
        List<ProductTable> getAllProducts();
    }
}
