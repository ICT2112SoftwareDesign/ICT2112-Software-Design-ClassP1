using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CleanBrilliantCompany.Models;

[Route("CostPage")] // ✅ This sets the base route
public class CostPageController : Controller
{
    // Control Class
    private readonly CostControl costControl;

    // Logger for the PC class
    private readonly ILogger<CostPageController> logger;

    public CostPageController(
        ILogger<CostPageController> logger, 
        CostControl costControl
        ) 
        {
            this.logger = logger;
            this.costControl = costControl;
        }

    [HttpGet("")]
    public IActionResult Index()
    {
        var latestDashboard = costControl.GetLatestDashboard();
        if (latestDashboard == null)
        {
            logger.LogWarning("No dashboard found.");
            return View();
        }
        return View(latestDashboard);
    }

    [HttpPost("GenerateDashboard")]
    public IActionResult GenerateDashboard()
    {
        var latest = costControl.GetLatestDashboard();
        if (latest?.GeneratedDate != null && latest.GeneratedDate.Value.Date == DateTime.Today)
        {
            logger.LogInformation("📅 A dashboard has already been created today. No new dashboard generated.");
            return Json(new { message = "📅 Dashboard is already up to date for today. No new dashboard created." });
        }

        costControl.UpdateDashboard();
        logger.LogInformation("📊 Dashboard creation/update requested.");

        return Json(new { message = "✅ Dashboard has been created or updated successfully." });
    }


    // MODIFIED -------------------------------------------------------------------------------------

    

    [HttpGet("GetCostVisualization")]
    public IActionResult GetCostVisualization()
    {
        var dashboard = costControl.GetLatestDashboard();
        if (dashboard == null)
        {
            return NotFound("No dashboard data available.");
        }
        return Json(dashboard.GenerateCostVisualization());
    }

    // ✅ Call Dashboard Method for Supplier Comparison
    [HttpGet("GetSupplierComparison")]
    public IActionResult GetSupplierComparison()
    {
        var dashboard = costControl.GetLatestDashboard();
        if (dashboard == null)
        {
            return NotFound("No dashboard data available.");
        }
        // return Json(dashboard.GenerateSupplierComparison());
        return Json(dashboard.GetSupplierComparison(costControl.DbContext));    // ✅ Call method to get Supplier Comparison
}

    [HttpGet("GetAlerts")]
    public IActionResult GetAlerts()
    {
       var dashboard = costControl.GetLatestDashboard();
        if (dashboard == null)
        {
            return NotFound(new { message = "No dashboard data available." });
        }

        var summary = dashboard.GetBatchBudgetSummary();
        return Json(summary);  // ✅ Returns total cost & exceeded amount
    }

    [HttpGet("GetProductAlerts")]
    public IActionResult GetProductAlerts(int productId)
    {
        var dashboard = costControl.GetLatestDashboard();
        if (dashboard == null)
        {
            return NotFound(new { message = "No dashboard data available." });
        }

        var productAlerts = dashboard.CheckProductPerformance(productId); // ✅ Fetch alerts for specific ProductID
        return Json(productAlerts);  
    }

    [HttpGet("GetBatchesByManufacturer")]
    public IActionResult GetBatchesByManufacturer(int manufacturerId)
    {
        logger.LogInformation($"[DEBUG] API received manufacturerId: {manufacturerId}");

        var dashboard = costControl.GetLatestDashboard();
        if (dashboard == null)
        {
            logger.LogWarning("No dashboard data available.");
            return NotFound(new { message = "No dashboard data available." });
        }

        var batches = dashboard.GetBatchesByManufacturer(manufacturerId);
        
        logger.LogInformation($"[DEBUG] Found {batches.Count} batches for Manufacturer ID: {manufacturerId}");

        return Json(batches);
    }

    [HttpGet("GetManufacturerById")]
    public IActionResult GetManufacturerById(int manufacturerId)
    {
        var dashboard = costControl.GetLatestDashboard();
        if (dashboard == null)
        {
            return NotFound("No dashboard data available.");
        }

        var manufacturer = dashboard.GetAllManufacturers()
            .FirstOrDefault(m => m.ManufacturerId == manufacturerId);

        if (manufacturer == null)
        {
            return NotFound($"Manufacturer with ID {manufacturerId} not found.");
        }

        return Json(manufacturer);
    }


    [HttpGet("GetAvgBatchPriceByManufacturer")]
    public IActionResult GetAvgBatchPriceByManufacturer()
    {
        var dashboard = costControl.GetLatestDashboard();
        if (dashboard == null)
        {
            return NotFound("No dashboard data available.");
        }

        var result = dashboard.GetAvgBatchCost(costControl.DbContext);
        return Json(result);
    }

    [HttpGet("GetManufacturerBatchCount")]
    public IActionResult GetManufacturerBatchCount()
    {
        var dashboard = costControl.GetLatestDashboard();
        if (dashboard == null)
        {
            return NotFound("No dashboard data available.");
        }

        var batchCounts = dashboard.GetAllManufacturers()
            .Select(manufacturer => new
            {
                manufacturer.ManufacturerId,
                manufacturer.CompanyName,
                BatchCount = dashboard.GetBatchesByManufacturer(manufacturer.ManufacturerId).Count
            }).ToList();

        return Json(batchCounts);
    }

    [HttpGet("GetBatchManufacturer")]
    public IActionResult GetBatchManufacturer(int batchCode)
    {
        var dashboard = costControl.GetLatestDashboard();
        if (dashboard == null)
        {
            return NotFound("No dashboard data available.");
        }

        var manufacturer = dashboard.GetManufacturerForBatch(batchCode);
        if (manufacturer == null)
        {
            return NotFound($"Manufacturer for batch {batchCode} not found.");
        }

        return Json(manufacturer);
    }

    [HttpGet("GetBatchDetails")]
    public IActionResult GetBatchDetails(int batchCode)
    {
        var dashboard = costControl.GetLatestDashboard();
        if (dashboard == null)
        {
            return NotFound("No dashboard data available.");
        }

        var batch = dashboard.GetAllProductBatches()
            .FirstOrDefault(b => b.BatchCode == batchCode);

        if (batch == null)
        {
            return NotFound($"Batch with code {batchCode} not found.");
        }

        return Json(batch);
    }

    // [HttpGet("GetCheapestAndMostExpensiveOverview")]
    // public IActionResult GetCheapestAndMostExpensiveOverview()
    // {
    //     var dashboard = costControl.GetLatestDashboard();
    //     if (dashboard == null)
    //     {
    //         return NotFound("No dashboard data available.");
    //     }

    //     // Get suppliers sorted by avg batch price
    //     var suppliers = dashboard.GetAllManufacturers()
    //         .Select(manufacturer => new
    //         {
    //             ManufacturerId = manufacturer.ManufacturerId,
    //             CompanyName = manufacturer.CompanyName,
    //             AvgBatchPrice = dashboard.GetBatchesByManufacturer(manufacturer.ManufacturerId)
    //                                     .Any() ? dashboard.GetBatchesByManufacturer(manufacturer.ManufacturerId)
    //                                     .Average(b => b.BatchPrice) : 0
    //         })
    //         .OrderBy(m => m.AvgBatchPrice)
    //         .ToList();

    //     var cheapestSupplier = suppliers.FirstOrDefault();
    //     var mostExpensiveSupplier = suppliers.LastOrDefault();

    //     // Get batches sorted by batch price
    //     var batches = dashboard.GetAllProductBatches()
    //         .OrderBy(b => b.BatchPrice)
    //         .ToList();

    //     var cheapestBatch = batches.FirstOrDefault();
    //     var mostExpensiveBatch = batches.LastOrDefault();

    //     return Json(new
    //     {
    //         CheapestSupplier = cheapestSupplier,
    //         MostExpensiveSupplier = mostExpensiveSupplier,
    //         CheapestBatch = cheapestBatch,
    //         MostExpensiveBatch = mostExpensiveBatch
    //     });
    // }


    // This one returns only the DashboardBatchSummaryDTO
    // [HttpGet("GetCheapestAndMostExpensiveOverview")]
    // public IActionResult GetCheapestAndMostExpensiveOverview()
    // {
    //     var dashboard = costControl.GetLatestDashboard();
    //     if (dashboard == null)
    //     {
    //         return NotFound("No dashboard data available.");
    //     }

    //     // Call the method in the dashboard that checks the DB,
    //     // generates a new overview if necessary, and returns a DTO.
    //     var overview = dashboard.GetCheapestAndMostExpensiveOverviewDetailed(costControl.DbContext);
    //     return Json(overview);
    // }


    [HttpGet("GetCheapestAndMostExpensiveOverview")]
    public IActionResult GetCheapestAndMostExpensiveOverview()
    {
        var dashboard = costControl.GetLatestDashboard();
        if (dashboard == null)
        {
            return NotFound("No dashboard data available.");
        }

        var overview = dashboard.GetCheapestAndMostExpensiveOverviewDetailed(costControl.DbContext);
        return Json(overview);
    }


 [HttpGet("GetCheapestAndMostExpensiveByManufacturer")]
    public IActionResult GetCheapestAndMostExpensiveByManufacturer(int manufacturerId)
    {
        var dashboard = costControl.GetLatestDashboard();
        if (dashboard == null)
        {
            return NotFound("No dashboard data available.");
        }

        // Cast the result to dynamic list so LINQ can be used
        var batches = (dashboard.GenerateCostVisualizationByManufacturer(manufacturerId) as IEnumerable<dynamic>)?.ToList();

        if (batches == null || !batches.Any())
        {
            return NotFound("No batches found for this manufacturer.");
        }

        var cheapestBatch = batches.OrderBy(b => (decimal)b.BatchPrice).First();
        var mostExpensiveBatch = batches.OrderByDescending(b => (decimal)b.BatchPrice).First();

        return Json(new
        {
            CheapestBatchCode = cheapestBatch.BatchCode,
            CheapestBatchPrice = cheapestBatch.BatchPrice,
            MostExpensiveBatchCode = mostExpensiveBatch.BatchCode,
            MostExpensiveBatchPrice = mostExpensiveBatch.BatchPrice
        });
    }

    // [HttpGet("GetCheapestAndMostExpensiveByManufacturer")]
    // public IActionResult GetCheapestAndMostExpensiveByManufacturer(int manufacturerId)
    // {
    //     var dashboard = costControl.GetLatestDashboard();
    //     if (dashboard == null)
    //     {
    //         return NotFound("No dashboard data available.");
    //     }

    //     var batches = dashboard.GetBatchesByManufacturer(manufacturerId);
    //     if (!batches.Any())
    //     {
    //         return NotFound("No batches found for this manufacturer.");
    //     }

    //     var cheapestBatch = batches.OrderBy(b => b.BatchPrice).First();
    //     var mostExpensiveBatch = batches.OrderByDescending(b => b.BatchPrice).First();

    //     return Json(new
    //     {
    //         cheapestBatch,
    //         mostExpensiveBatch
    //     });
    // }

    

    [HttpGet("GetRecentBatchesByManufacturer")]
    public IActionResult GetRecentBatchesByManufacturer(int manufacturerId)
    {
        var dashboard = costControl.GetLatestDashboard();
        if (dashboard == null)
        {
            return NotFound("No dashboard data available.");
        }

        var recentBatches = dashboard.GetBatchesByManufacturer(manufacturerId)
            .OrderByDescending(b => b.ReceiveDate)
            .Take(5)
            .ToList();

        return Json(recentBatches);
    }

    [HttpGet("GetAllProducts")]
    public IActionResult GetAllProducts()
    {
        var dashboard = costControl.GetLatestDashboard();
        if (dashboard == null)
        {
            return NotFound("No dashboard data available.");
        }

        var products = dashboard.GetAllProducts();
        return Json(products);
    }

    [HttpGet("GetBatchesByProduct")]
    public IActionResult GetBatchesByProduct(int productId)
    {
        var dashboard = costControl.GetLatestDashboard();
        if (dashboard == null)
        {
            return NotFound("No dashboard data available.");
        }

        var batches = dashboard.GetBatchesByProduct(productId);
        return Json(batches);
    }

    [HttpGet("GetCheapestAndMostExpensiveByProduct")]
    public IActionResult GetCheapestAndMostExpensiveByProduct(int productId)
    {
        var dashboard = costControl.GetLatestDashboard();
        if (dashboard == null)
        {
            return NotFound("No dashboard data available.");
        }

        var batches = dashboard.GetBatchesByProduct(productId);

        return Json(new
        {
            cheapestBatch = batches.OrderBy(b => b.BatchPrice).First(),
            mostExpensiveBatch = batches.OrderByDescending(b => b.BatchPrice).First()
        });
    }

    [HttpGet("GetRecentBatchesByProduct")]
    public IActionResult GetRecentBatchesByProduct(int productId)
    {
        var dashboard = costControl.GetLatestDashboard();
        if (dashboard == null)
        {
            return NotFound("No dashboard data available.");
        }

        var recentBatches = dashboard.GetBatchesByProduct(productId)
            .OrderByDescending(b => b.ReceiveDate)
            .Take(5)
            .ToList();

        return Json(recentBatches);
    }

    
}
