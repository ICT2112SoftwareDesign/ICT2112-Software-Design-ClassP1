using CleanBrilliantCompany.DTO;

public class CostDashboardRdm : Dashboard
{
    private ILogger<CostDashboardRdm>? logger;
    private IVisualizationService? visualizationService;
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
        IVisualizationService visualizationService,
        IAlertService alertService)
    {
        this.logger = logger;
        this.visualizationService = visualizationService;
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

    // // 🔹 Get Batches for a Manufacturer ID
    public List<ProductBatchDTO> GetBatchesByManufacturer(int manufacturerId)
    {
        return batchDetails.Where(b => b.ManufacturerId == manufacturerId).ToList();
    }

    // // 🔹 Get Batches for a Specific Product ID
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

    public List<int> GetAllProducts()
    {
        return batchDetails
            .Select(b => b.ProductId)
            .Distinct()
            .ToList();
    }

    //////////////////////////////////////////

    //////////////////////////////////////////
    // Below are the methods that will be called from the concrete interface for the visualization service

    // 🔹 Generate Cost Visualization Data (returns JSON-ready data)
    public object GenerateCostVisualization()
    {
        return visualizationService!.GenerateCostVisualization(this);
    }

    // 🔹 Generate Supplier Comparison Data (returns JSON-ready data)
    public object GenerateSupplierComparison()
    {
        return visualizationService!.GenerateSupplierComparison(this);
    }
    
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
