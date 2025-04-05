public class InventoryControl
{
    private readonly ApplicationDbContext _context;
    //private readonly SimulatedDbContext _simulatedContext;
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IProduct _productService;

    public InventoryControl(ApplicationDbContext context, IInventoryRepository inventoryRepository, IProduct productService)
    {
        _inventoryRepository = inventoryRepository ?? throw new ArgumentNullException(nameof(inventoryRepository));
        _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        _context = context ?? throw new ArgumentNullException(nameof(context));

    }

    public void CreateDashboard(string name, int validityDuration)
    {
        var newDashboard = new InventoryDashboardRDM(name, validityDuration);

        // Get all products
        var allProducts = _productService.getAllProducts();

        // Extract productId and quantity
        var stockLevels = allProducts.ToDictionary(
            p =>
            {
                var info = p.retrieveProductInfo();
                return (int)info["ProductId"];
            },
            p =>
            {
                var info = p.retrieveProductInfo();
                return (int)info["Quantity"];
            }
        );

        var dbThresholds = _inventoryRepository.GetAllProductThresholds();

        var thresholds = allProducts.ToDictionary(
            p =>
            {
                var info = p.retrieveProductInfo();
                return (int)info["ProductId"];
            },
            p =>
            {
                var info = p.retrieveProductInfo();
                var id = (int)info["ProductId"];
                return dbThresholds.ContainsKey(id) ? dbThresholds[id] : 100;
            }
        );

        // Update dashboard with current data
        newDashboard.UpdateStockThreshold(
            stockLevels,
            thresholds.ToDictionary(kvp => kvp.Key, kvp => kvp.Value ?? 100)
        );
        newDashboard.UpdateReplenishmentStatus();
        newDashboard.GenerateAlerts(); // Generate alerts after updating data

        // Save the dashboard with the updated data
        _inventoryRepository.SaveDashboard(newDashboard);
    }

    public InventoryDashboardRDM FetchDashboard()
    {
        var dashboard = _inventoryRepository.GetLatestDashboard();
        if (dashboard == null)
        {
            throw new InvalidOperationException("No dashboard available. Please create a dashboard first.");
        }

        return dashboard;
    }

    public string GenerateReport()
    {
        var dashboard = FetchDashboard();
        var lowStock = CheckLowStock();
        var overStock = CheckOverStock();
        var alertStreaks = GetWeeklyConsecutiveAlertCounts();

        var report = new StringBuilder();
        report.AppendLine($"<h1>Inventory Report - {dashboard.Name}</h1>");
        report.AppendLine($"<p>Period: {dashboard.RequestedStartDate} to {dashboard.RequestedEndDate}</p>");
        report.AppendLine("<h2>Stock Levels</h2><ul>");
        foreach (var stock in dashboard.GetAllStockLevels())
        {
            report.AppendLine($"<li>Product {stock.Key}: {stock.Value} units (Threshold: {dashboard.GetThreshold(stock.Key)})</li>");
        }
        report.AppendLine("</ul>");
        report.AppendLine("<h2>Low Stock Products</h2><ul>");
        foreach (var productId in lowStock)
        {
            report.AppendLine($"<li>Product {productId}: {dashboard.GetStockLevel(productId)}</li>");
        }
        report.AppendLine("</ul>");
        report.AppendLine("<h2>Over Stock Products</h2><ul>");
        foreach (var productId in overStock)
        {
            report.AppendLine($"<li>Product {productId}: {dashboard.GetStockLevel(productId)}</li>");
        }
        report.AppendLine("</ul>");
        report.AppendLine("<h2>Consecutive Weekly Alerts</h2><ul>");
        foreach (var productId in alertStreaks.Keys)
        {
            var streaks = alertStreaks[productId];
            if (streaks.LowStockWeeks > 0)
            {
                report.AppendLine($"<li>Product {productId}: Low Stock for {streaks.LowStockWeeks} week(s) in a row</li>");
            }
            if (streaks.OverStockWeeks > 0)
            {
                report.AppendLine($"<li>Product {productId}: Over Stock for {streaks.OverStockWeeks} week(s) in a row</li>");
            }
        }
        report.AppendLine("</ul>");

        Console.WriteLine("Report generated.");
        return report.ToString();
    }

    public List<int> CheckLowStock(string category = null)
    {
        var dashboard = FetchDashboard();
        var allStockLevels = dashboard.GetAllStockLevels();
        var thresholds = dashboard.GetAllThresholds();
        var products = _productService.getAllProducts();

        if (!string.IsNullOrEmpty(category))
        {
            products = products.Where(p =>
            {
                var info = p.retrieveProductInfo();
                return (string)info["ProductCategory"] == category;
            }).ToList();
        }

        return products
            .Where(p =>
            {
                var info = p.retrieveProductInfo();
                var id = (int)info["ProductId"];
                return allStockLevels.ContainsKey(id)
                    && thresholds.ContainsKey(id)
                    && allStockLevels[id] < thresholds[id] * 0.35;
            })
            .Select(p => (int)p.retrieveProductInfo()["ProductId"])
            .ToList();

    }

    public List<int> CheckOverStock(string category = null)
    {
        var dashboard = FetchDashboard();
        var allStockLevels = dashboard.GetAllStockLevels();
        var thresholds = dashboard.GetAllThresholds();
        var products = _productService.getAllProducts();

        if (!string.IsNullOrEmpty(category))
        {
            products = products.Where(p =>
            {
                var info = p.retrieveProductInfo();
                return (string)info["ProductCategory"] == category;
            }).ToList();
        }

        return products
            .Where(p =>
            {
                var info = p.retrieveProductInfo();
                var id = (int)info["ProductId"];
                return allStockLevels.ContainsKey(id)
                    && thresholds.ContainsKey(id)
                    && allStockLevels[id] > thresholds[id] * 1.6;
            })
            .Select(p =>
            {
                var info = p.retrieveProductInfo();
                return (int)info["ProductId"];
            })
            .ToList();
    }


    public List<int> GenerateReplenishmentActions(string category = null)
    {
        return CheckLowStock(category);
    }

    public string GenerateStockLevelChartData(string category = null)
    {
        var dashboard = FetchDashboard();
        var stockLevels = dashboard.GetAllStockLevels();
        var thresholds = _context.ProductThresholdTable
            .ToDictionary(t => t.ProductId, t => t.Threshold ?? 100);
        var productIds = stockLevels.Keys.ToList();

        var products = _productService.getAllProducts()
            .Where(p =>
            {
                var info = p.retrieveProductInfo();
                return productIds.Contains((int)info["ProductId"]);
            })
            .ToList();

        if (!string.IsNullOrEmpty(category))
        {
            products = products.Where(p =>
            {
                var info = p.retrieveProductInfo();
                return (string)info["ProductCategory"] == category;
            }).ToList();
        }

        var chartData = new
        {
            labels = products.Select(p =>
            {
                var info = p.retrieveProductInfo();
                return (int)info["ProductId"];
            }).ToList(),
            datasets = new[] {
            new {
                label = "Stock Levels",
                data = products.Select(p =>
                {
                    var info = p.retrieveProductInfo();
                    return stockLevels[(int)info["ProductId"]];
                }).ToList()
            },
            new {
                label = "Thresholds",
                data = products.Select(p =>
                {
                    var info = p.retrieveProductInfo();
                    return thresholds[(int)info["ProductId"]];
                }).ToList()
            }
        },
            products = products.Select(p =>
            {
                var info = p.retrieveProductInfo();
                var id = (int)info["ProductId"];
                return new
                {
                    id = id,
                    name = (string)info["ProductName"],
                    category = (string)info["ProductCategory"],
                    stock = stockLevels[id],
                    threshold = thresholds[id]
                };
            }).ToList()
        };

        return System.Text.Json.JsonSerializer.Serialize(chartData);
    }

    public Dictionary<int, (int LowStockWeeks, int OverStockWeeks)> GetWeeklyConsecutiveAlertCounts()
    {
        return _inventoryRepository.GetConsecutiveWeeklyAlerts();
    }

    public Dictionary<int, InventoryDTO> GetProductLookup()
    {
        var dashboard = FetchDashboard();
        var stockLevels = dashboard.GetAllStockLevels();
        var thresholds = dashboard.GetAllThresholds();
        var replenishmentStatuses = dashboard.GetAllReplenishmentStatuses();

        return _productService.getAllProducts()
            .ToDictionary(p =>
            {
                var info = p.retrieveProductInfo();
                return (int)info["ProductId"];
            },
            p =>
            {
                var info = p.retrieveProductInfo();
                var id = (int)info["ProductId"];

                return new InventoryDTO
                {
                    ProductId = id,
                    ProductName = (string)info["ProductName"],
                    ProductCategory = (string)info["ProductCategory"],
                    StockLevel = stockLevels.ContainsKey(id) ? stockLevels[id] : 0,
                    Threshold = thresholds.ContainsKey(id) ? thresholds[id] : 100,
                    ReplenishmentStatus = replenishmentStatuses.ContainsKey(id) && replenishmentStatuses[id],
                };
            });
    }

    // Get product categories
    public List<string> GetProductCategories()
    {
        return _productService.getAllProducts()
            .Select(p =>
            {
                var info = p.retrieveProductInfo();
                return (string)info["ProductCategory"];
            })
            .Distinct()
            .ToList();
    }

    public InventoryDTO GetThresholdById(int id)
    {
        var product = _productService.getAllProducts()
            .FirstOrDefault(p => (int)p.retrieveProductInfo()["ProductId"] == id);

        var threshold = _context.ProductThresholdTable
            .FirstOrDefault(t => t.ProductId == id);

        var info = product?.retrieveProductInfo();

        return new InventoryDTO
        {
            ProductId = id,
            ProductName = info != null ? (string)info["ProductName"] : "Unknown",
            ProductCategory = info != null ? (string)info["ProductCategory"] : "Unknown",
            Threshold = threshold?.Threshold ?? 100,
            LastUpdated = threshold?.LastUpdated
        };
    }



    public void UpdateThreshold(ProductThresholdTable model)
    {
        var entry = _context.ProductThresholdTable.FirstOrDefault(t => t.ProductId == model.ProductId);
        if (entry != null)
        {
            entry.Threshold = model.Threshold;
            entry.LastUpdated = DateTime.Now;
        }
        else
        {
            _context.ProductThresholdTable.Add(model);
        }

        _context.SaveChanges();
    }
    public List<InventoryDTO> GetProductThresholdsWithInfo()
    {
        var products = _productService.getAllProducts();

        var productDict = products.ToDictionary(
            p => (int)p.retrieveProductInfo()["ProductId"],
            p => p.retrieveProductInfo()
        );

        var thresholds = _context.ProductThresholdTable.ToList();

        return thresholds
            .Where(pt => productDict.ContainsKey(pt.ProductId))
            .Select(pt =>
            {
                var info = productDict[pt.ProductId];
                return new InventoryDTO
                {
                    ProductId = pt.ProductId,
                    ProductName = (string)info["ProductName"],
                    Threshold = pt.Threshold ?? 100,
                    LastUpdated = pt.LastUpdated
                };
            })
            .ToList();
    }

    public void InitializeMissingThresholds()
    {
        // Get all product IDs
        var allProductIds = _productService.getAllProducts()
            .Select(p => (int)p.retrieveProductInfo()["ProductId"])
            .ToList();

        // Get product IDs that already have thresholds
        var existingThresholdProductIds = _context.ProductThresholdTable
            .Select(pt => pt.ProductId)
            .ToList();

        // Find missing product IDs
        var missingProductIds = allProductIds
            .Except(existingThresholdProductIds)
            .ToList();

        // Insert default thresholds (100) for missing products
        foreach (var productId in missingProductIds)
        {
            _context.ProductThresholdTable.Add(new ProductThresholdTable
            {
                ProductId = productId,
                Threshold = 100,
                LastUpdated = DateTime.Now
            });
        }

        _context.SaveChanges();
    }
}