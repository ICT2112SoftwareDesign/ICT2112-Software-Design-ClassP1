using CleanBrilliantCompany.Controllers;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Mappers;

public class AgingControl 
{

    private readonly iProductQuery _productQuery;

    public AgingControl(iProductQuery productQuery)
    {
        _productQuery = productQuery; 
    }

    public async Task generateNewDashboard() 
    {
        var batches = await _productQuery.getAllProductBatch();
        var specificBatch = await _productQuery.getBatchDetails(1); //Example batchCode 1
        var (status, stockHistories) = await _productQuery.getStockHistoryByBatch(1);
        Console.WriteLine("Product Batches:");
        foreach (var batch in batches)
        {
            Console.WriteLine($"Batch Code: {batch.BatchCode}, Product ID: {batch.ProductId}");
        }

        if (specificBatch != null)
        {
            Console.WriteLine($"Specific Batch Code: {specificBatch.BatchCode}, Product ID: {specificBatch.ProductId}");
            foreach (var stockHistory in stockHistories)
            {
                Console.WriteLine($"Stock Id: {stockHistory.StockId}, Batch Code: {stockHistory.BatchCode}");
            }
        }
    }
}