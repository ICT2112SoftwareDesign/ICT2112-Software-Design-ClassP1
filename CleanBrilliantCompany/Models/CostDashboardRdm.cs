using CleanBrilliantCompany.DTO;

public class CostDashboardRdm : Dashboard
{
    private ILogger<CostDashboardRdm>? logger;
    // private IVisualizationService? visualizationService;
    private IAlertService? alertService;
    private AbstractCostDetails? costDetails;  // Using Abstract Class

    public string Name { get; }
    public DateTime StartDate { get; }
    public DateTime EndDate { get; }
    public int ValidityDuration { get; }
    public int Type { get; }

    private List<ProductBatchDTO> batchDetails = new();
    private List<ProductManufacturerDTO> manufacturerDetails = new();
    private List<ItemDTO> itemDetails = new();

    public CostDashboardRdm(
        int id,
        string name,
        DateTime requestedStartDate,
        DateTime requestedEndDate,
        int validityDuration,
        int type,
        DateTime? generatedDate = null
    ) : base(id, name, requestedStartDate, requestedEndDate, validityDuration, type, generatedDate)
    {
        Name = name;
        StartDate = requestedStartDate;
        EndDate = requestedEndDate;
        ValidityDuration = validityDuration;
        Type = type;
    }
        
    public void InitializeServices(
        ILogger<CostDashboardRdm> logger,
        // IVisualizationService visualizationService,
        IAlertService alertService)
    {
        this.logger = logger;
        // this.visualizationService = visualizationService;
        this.alertService = alertService;

        this.costDetails = new ConcreteCostDetails(alertService, batchDetails, itemDetails);
    }

    public void ProcessManufacturers(List<ProductManufacturerDTO> manufacturers)
    {
        manufacturerDetails.AddRange(manufacturers);
    }

    public void ProcessProductBatches(List<ProductBatchDTO> productBatches)
    {
        batchDetails.AddRange(productBatches);
    }

    public void ProcessItems(List<ItemDTO> items)
    {
        itemDetails.AddRange(items);
    }

    //////////////////////////////////////////
    /// Cost Control Methods
     
    // 🔹 Get All Manufacturers
    public List<ProductManufacturerDTO> GetAllManufacturers()
    {
        return manufacturerDetails.ToList(); // ✅ Works with List<T>
    }

    // 🔹 Get All Product Batches
    public List<ProductBatchDTO> GetAllProductBatches()
    {
        return batchDetails.ToList(); // ✅ Works with List<T>
    }

    public List<ItemDTO> GetAllItemBatches()
    {
        return itemDetails.ToList(); // ✅ Works with List<T>
    }

    // 🔹 Get Manufacturer Info for a Batch
    public ProductManufacturerDTO? GetManufacturerForBatch(int batchCode)
    {
        var batch = batchDetails.FirstOrDefault(b => b.BatchCode == batchCode);
        if (batch == null)
        {
            logger!.LogWarning($"Batch with code {batchCode} not found.");
            return null;
        }

        var manufacturer = manufacturerDetails.FirstOrDefault(m => m.ManufacturerId == batch.ManufacturerId);
        if (manufacturer == null)
        {
            logger!.LogWarning($"Manufacturer with ID {batch.ManufacturerId} not found for batch {batchCode}.");
            return null;
        }

        return manufacturer;
    }

    // 🔹 Get Batches for a Manufacturer ID
    public List<ProductBatchDTO> GetBatchesByManufacturer(int manufacturerId)
    {
        return batchDetails.Where(b => b.ManufacturerId == manufacturerId).ToList();
    }

    // 🔹 Get Batches for a Specific Product ID
    public List<ProductBatchDTO> GetBatchesByProduct(int productId)
    {
        var batches = batchDetails.Where(b => b.ProductId == productId).ToList();

        // Debugging output to check if batches are being filtered correctly
        Console.WriteLine($"[DEBUG] Searching for batches with Product ID: {productId}");
        Console.WriteLine($"[DEBUG] Found {batches.Count} batches for Product ID: {productId}");

        foreach (var batch in batches)
        {
            Console.WriteLine($"[DEBUG] Batch Code: {batch.BatchCode}, Product ID: {batch.ProductId}, Price: {batch.BatchPrice}, Quantity: {batch.BatchQuantity}");
        }

        return batches;
    }

    // // 🔹 Get Manufacturer Info
    public ProductManufacturerDTO? GetManufacturerById(int manufacturerId)
    {
        return manufacturerDetails.FirstOrDefault(m => m.ManufacturerId == manufacturerId);
    }


    // 🔹 Get All Products
    public List<int> GetAllProducts()
    {
        return batchDetails
            .Select(b => b.ProductId)
            .Distinct()
            .ToList();
    }


    // 🔹 Get Supplier Comparison Data
    public List<ManufacturerAnalyticsTable> GetSupplierComparison(ApplicationDbContext dbContext)
    {
        var records = dbContext.ManufacturerAnalytics
            .Where(a => a.DashboardId == this.DashboardId)
            .Select(a => a)
            .ToList();

        if (records.Any())
        {
            Console.WriteLine($"[DEBUG] Retrieved {records.Count} records from DB for DashboardID: {this.DashboardId}");
            return records;
        }

        // No records found, generate and insert
        Console.WriteLine($"[DEBUG] No data found for DashboardID: {this.DashboardId}. Generating new analytics...");
        var data = this.GenerateSupplierComparison();

        foreach (var entry in data)
        {
            dbContext.ManufacturerAnalytics.Add(new ManufacturerAnalyticsTable
            {
                DashboardId = this.DashboardId,
                ManufacturerId = entry.ManufacturerId,
                ManufacturerName = entry.ManufacturerName,
                BatchCount = entry.BatchCount
            });

            Console.WriteLine($"[INSERTED] ManufacturerAnalytics - DashboardID: {this.DashboardId}, ManufacturerID: {entry.ManufacturerId}, Name: {entry.ManufacturerName}, BatchCount: {entry.BatchCount}");
        }

        dbContext.SaveChanges();
        Console.WriteLine(dbContext.SaveChanges());
        return data.Select(entry => new ManufacturerAnalyticsTable
        {
            DashboardId = this.DashboardId,
            ManufacturerId = entry.ManufacturerId,
            ManufacturerName = entry.ManufacturerName,
            BatchCount = entry.BatchCount
        }).ToList();
    }


    // 🔹 Generate Supplier Comparison Data + Adds Batch Count

    private List<ManufacturerAnalyticsDTO> GenerateSupplierComparison()
    {
        return this.GetAllManufacturers()
            .Select(manufacturer => new ManufacturerAnalyticsDTO
            {
                ManufacturerId = manufacturer.ManufacturerId,
                ManufacturerName = manufacturer.CompanyName,
                BatchCount = this.GetBatchesByManufacturer(manufacturer.ManufacturerId).Count
            })
            .ToList();
    }

    // 🔹 Get Average Batch Cost by Manufacturer

    public List<ManufacturerCostAnalyticsDTO> GetAvgBatchCost(ApplicationDbContext dbContext)
    {
        // Try to retrieve existing records from the DB.
        var records = dbContext.ManufacturerCostAnalytics
            .Where(a => a.DashboardId == this.DashboardId)
            .Select(a => new ManufacturerCostAnalyticsDTO
            {
                ManufacturerId = a.ManufacturerId,
                ManufacturerName = a.ManufacturerName,
                // Explicitly cast the value from double to decimal
                AvgBatchCost = (decimal)(double)a.AvgBatchCost
            })
            .ToList();

        if (records.Any())
        {
            Console.WriteLine($"[DEBUG] Retrieved {records.Count} average batch cost records for DashboardID: {this.DashboardId}");
            return records;
        }

        Console.WriteLine($"[DEBUG] No average batch cost records found for DashboardID: {this.DashboardId}. Generating new analytics...");

        // Generate new analytics data using a DTO
        var data = this.GetAllManufacturers().Select(manufacturer =>
        {
            var batches = this.GetBatchesByManufacturer(manufacturer.ManufacturerId);
            decimal avgCost = 0m;
            if (batches.Any())
            {
                // Calculate average as a double then explicitly cast to decimal.
                double avgDouble = batches.Average(b => (double)b.BatchPrice);
                avgCost = (decimal)avgDouble;
            }
            return new ManufacturerCostAnalyticsDTO
            {
                ManufacturerId = manufacturer.ManufacturerId,
                ManufacturerName = manufacturer.CompanyName,
                AvgBatchCost = avgCost
            };
        }).ToList();

        // Insert the new analytics into the database.
        foreach (var entry in data)
        {
            dbContext.ManufacturerCostAnalytics.Add(new ManufacturerCostAnalyticsTable
            {
                DashboardId = this.DashboardId,
                ManufacturerId = entry.ManufacturerId,
                ManufacturerName = entry.ManufacturerName,
                AvgBatchCost = entry.AvgBatchCost
            });

            Console.WriteLine($"[INSERTED] AvgCost - DashboardID: {this.DashboardId}, ManufacturerID: {entry.ManufacturerId}, Name: {entry.ManufacturerName}, AvgBatchCost: {entry.AvgBatchCost}");
        }

        dbContext.SaveChanges();

        return data;
    }

    // 🔹 Get Cheapest and Most Expensive Batch Overview

    public DashboardBatchSummaryDTO GetCheapestAndMostExpensiveOverview(ApplicationDbContext dbContext)
    {
        // Try to retrieve an existing summary for this dashboard.
        var summaryEntity = dbContext.DashboardBatchSummary
            .FirstOrDefault(s => s.DashboardId == this.DashboardId);
        if (summaryEntity != null)
        {
            Console.WriteLine($"[DEBUG] Retrieved existing batch summary for DashboardID: {this.DashboardId}");
            return new DashboardBatchSummaryDTO
            {
                CheapestBatchCode = summaryEntity.CheapestBatchCode,
                ExpensiveBatchCode = summaryEntity.ExpensiveBatchCode,
                CheapestManufacturer = summaryEntity.CheapestManufacturer,
                ExpensiveManufacturer = summaryEntity.ExpensiveManufacturer
            };
        }

        Console.WriteLine($"[DEBUG] No batch summary found for DashboardID: {this.DashboardId}. Generating new summary...");

        // Generate new summary:
        // 1. Compute supplier overview.
        var suppliers = this.GetAllManufacturers()
            .Select(manufacturer => new
            {
                ManufacturerId = manufacturer.ManufacturerId,
                CompanyName = manufacturer.CompanyName,
                AvgBatchPrice = this.GetBatchesByManufacturer(manufacturer.ManufacturerId).Any()
                    ? this.GetBatchesByManufacturer(manufacturer.ManufacturerId).Average(b => b.BatchPrice)
                    : 0
            })
            .OrderBy(s => s.AvgBatchPrice)
            .ToList();

        var cheapestSupplier = suppliers.FirstOrDefault();
        var expensiveSupplier = suppliers.LastOrDefault();

        // 2. Compute batch overview.
        var batches = this.GetAllProductBatches()
            .OrderBy(b => b.BatchPrice)
            .ToList();
        var cheapestBatch = batches.FirstOrDefault();
        var expensiveBatch = batches.LastOrDefault();

        // Create new summary entity with the key values.
        var newSummary = new DashboardBatchSummaryTable
        {
            DashboardId = this.DashboardId,
            CheapestBatchCode = cheapestBatch != null ? cheapestBatch.BatchCode : 0,
            ExpensiveBatchCode = expensiveBatch != null ? expensiveBatch.BatchCode : 0,
            CheapestManufacturer = cheapestSupplier != null ? cheapestSupplier.ManufacturerId : 0,
            ExpensiveManufacturer = expensiveSupplier != null ? expensiveSupplier.ManufacturerId : 0
        };

        dbContext.DashboardBatchSummary.Add(newSummary);
        dbContext.SaveChanges();

        Console.WriteLine($"[INSERTED] Batch summary for DashboardID: {this.DashboardId}");

        // Return the new summary as a DTO.
        return new DashboardBatchSummaryDTO
        {
            CheapestBatchCode = newSummary.CheapestBatchCode,
            ExpensiveBatchCode = newSummary.ExpensiveBatchCode,
            CheapestManufacturer = newSummary.CheapestManufacturer,
            ExpensiveManufacturer = newSummary.ExpensiveManufacturer
        };
    }


    public object GetSupplierComparisonData(ApplicationDbContext dbContext)
    {
        var result = dbContext.ManufacturerAnalytics
            .Where(a => a.DashboardId == this.DashboardId)
            .Select(a => new {
                a.ManufacturerId,
                a.ManufacturerName,
                a.BatchCount
            })
            .ToList();

        return result;
    }

   public object GetCheapestAndMostExpensiveOverviewDetailed(ApplicationDbContext dbContext)
{
    // Get the summary from your existing overview method.
    var summary = this.GetCheapestAndMostExpensiveOverview(dbContext);

    // Retrieve full batch details based on the summary batch codes.
    var cheapestBatch = this.GetAllProductBatches().FirstOrDefault(b => b.BatchCode == summary.CheapestBatchCode);
    var mostExpensiveBatch = this.GetAllProductBatches().FirstOrDefault(b => b.BatchCode == summary.ExpensiveBatchCode);

    // Retrieve all ManufacturerCostAnalytics records for the current dashboard.
    // Materialize the data into memory first to perform conversion safely.
    var rawAnalytics = dbContext.ManufacturerCostAnalytics
        .Where(a => a.DashboardId == this.DashboardId)
        .ToList();

    // Convert the AvgBatchCost for each record from double (or float) to decimal.
    var manufacturerAnalytics = rawAnalytics.Select(a => new ManufacturerCostAnalyticsTable
    {
        DashboardId = a.DashboardId,
        ManufacturerId = a.ManufacturerId,
        ManufacturerName = a.ManufacturerName,
        // Use string conversion to force the double value to convert into a decimal.
        AvgBatchCost = Convert.ToDecimal(a.AvgBatchCost.ToString())
    }).ToList();

    // Find the manufacturer records matching the summary keys.
    var cheapestManufacturer = manufacturerAnalytics.FirstOrDefault(m => m.ManufacturerId == summary.CheapestManufacturer);
    var mostExpensiveManufacturer = manufacturerAnalytics.FirstOrDefault(m => m.ManufacturerId == summary.ExpensiveManufacturer);

    // Return an aggregated anonymous object with the detailed information.
    return new
    {
        Summary = new
        {
            summary.CheapestBatchCode,
            summary.ExpensiveBatchCode,
            summary.CheapestManufacturer,
            summary.ExpensiveManufacturer
        },
        CheapestBatch = cheapestBatch != null ? new
        {
            BatchCode = cheapestBatch.BatchCode,
            BatchPrice = cheapestBatch.BatchPrice,
            ProductId = cheapestBatch.ProductId
        } : null,
        MostExpensiveBatch = mostExpensiveBatch != null ? new
        {
            BatchCode = mostExpensiveBatch.BatchCode,
            BatchPrice = mostExpensiveBatch.BatchPrice,
            ProductId = mostExpensiveBatch.ProductId
        } : null,
        CheapestManufacturer = cheapestManufacturer != null ? new
        {
            ManufacturerId = cheapestManufacturer.ManufacturerId,
            ManufacturerName = cheapestManufacturer.ManufacturerName,
            AvgBatchCost = cheapestManufacturer.AvgBatchCost
        } : null,
        MostExpensiveManufacturer = mostExpensiveManufacturer != null ? new
        {
            ManufacturerId = mostExpensiveManufacturer.ManufacturerId,
            ManufacturerName = mostExpensiveManufacturer.ManufacturerName,
            AvgBatchCost = mostExpensiveManufacturer.AvgBatchCost
        } : null
    };
}

    // 🔹 Get Cheapest and Most Expensive Batches by Manufacturer

    public object GenerateCostVisualizationByManufacturer(int manufacturerId)
    {
        return this.GetBatchesByManufacturer(manufacturerId)
            .Select(batch => new
            {
                BatchCode = batch.BatchCode,
                BatchPrice = batch.BatchPrice
            })
            .ToList();
    }

    public object GenerateCostVisualization()
    {
        return this.GetAllProductBatches()
            .Select(batch => new
            {
                BatchCode = batch.BatchCode,
                BatchPrice = batch.BatchPrice
            })
            .ToList();
    }


    


    //////////////////////////////////////////


    //////////////////////////////////////////
    // Below are the methods that will be called from the concrete interface for the alert service

    // Demo to check if each batch exceeds $500
    public object GetBatchBudgetSummary()
    {
        return costDetails!.GetBatchBudgetSummary();  // ✅ Forward the call
    }

    public object CheckProductPerformance(int productId)
    {
        Console.WriteLine($"[DEBUG] Checking performance for Product ID: {productId}");

        return costDetails!.CheckProductPerformance(productId);  // ✅ Forward call to costDetails
    }
    
    public List<Alert> GetAlerts()
    {
        return costDetails!.GetAlerts();  // ✅ Fetch alerts from costDetails
    }
}
