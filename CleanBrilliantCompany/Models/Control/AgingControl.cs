using CleanBrilliantCompany.Controllers;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Mappers;

public class AgingControl 
{

    // private readonly iProductQuery _productQuery;

    // public AgingControl(iProductQuery productQuery)
    // {
    //     _productQuery = productQuery; 
    // }

    // public async Task generateNewDashboard() 
    // {
    //     var batches = await _productQuery.getAllProductBatch();
    //     var specificBatch = await _productQuery.getBatchDetails(1); //Example batchCode 1
    //     var (status, stockHistories) = await _productQuery.getStockHistoryByBatch(1);
    //     Console.WriteLine("Product Batches:");
    //     foreach (var batch in batches)
    //     {
    //         Console.WriteLine($"Batch Code: {batch.BatchCode}, Product ID: {batch.ProductId}");
    //     }

    //     if (specificBatch != null)
    //     {
    //         Console.WriteLine($"Specific Batch Code: {specificBatch.BatchCode}, Product ID: {specificBatch.ProductId}");
    //         foreach (var stockHistory in stockHistories)
    //         {
    //             Console.WriteLine($"Stock Id: {stockHistory.StockId}, Batch Code: {stockHistory.BatchCode}");
    //         }
    //     }
    // }

    private readonly IProduct _productInterface;

    public AgingControl(IProduct productInterface)
    {
        _productInterface = productInterface; 
    }

    public void testProductInterfaceMethods()
    {
        var products = _productInterface.getAllProducts();
        
        for (int i = 0; i < products.Count; i++)
        {
            var product = products[i];
            Console.WriteLine($"Product Name: {product.ProductName}");
            Console.WriteLine($"Product Category: {product.ProductCategory}");
            Console.WriteLine($"Product Cost: ${product.ProductCost}");
            Console.WriteLine($"Manufacturer ID: {product.ManufacturerId}");
            Console.WriteLine($"Product Weight: {product.ProductWeight} kg");
            Console.WriteLine($"Product Quantity: {product.Quantity}");
            Console.WriteLine($"Product Volume: {product.Volume}");
            Console.WriteLine($"Toxicity Percentage: {product.ToxicityPercentage}%");
            Console.WriteLine($"Carbon Footprint: {product.CarbonFootprint}");
            Console.WriteLine($"Product State: {product.ProductState}");
            Console.WriteLine("---------------------------");  // To separate each product's details
        }
    }
}