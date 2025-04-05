using CleanBrilliantCompany.DatabaseEntities;
using Microsoft.CodeAnalysis;

public class InventoryMapper : IInventoryRepository
{
    private readonly ApplicationDbContext _context;

    public InventoryMapper(ApplicationDbContext context)
    {
        _context = context;
    }

    public void SaveDashboard(InventoryDashboardRDM dashboard)
    {
        using (var transaction = _context.Database.BeginTransaction())
        {
            try
            {
                var now = DateTime.Now;

                // Save the dashboard metadata
                var dashboardEntity = new DashboardTable
                {
                    Name = dashboard.Name,
                    RequestedStartDate = now,
                    RequestedEndDate = now,
                    GeneratedDate = now,
                    ValidityDuration = dashboard.ValidityDuration,
                    TypeId = 2
                };
                _context.DashboardTable.Add(dashboardEntity);
                _context.SaveChanges();


                // Save stock levels and thresholds to InventoryLevel
                var stockLevels = dashboard.GetAllStockLevels();
                var replenishmentStatuses = dashboard.GetAllReplenishmentStatuses();

                // Dictionary to map ProductId to InventoryId
                var productToInventoryIdMap = new Dictionary<int, int>();

                foreach (var productId in stockLevels.Keys)
                {
                    var inventoryLevel = new InventoryLevelTable
                    {
                        DashboardId = dashboardEntity.DashboardId,
                        ProductId = productId,
                        StockLevel = stockLevels[productId],
                        ReplenishmentStatus = replenishmentStatuses[productId]
                    };
                    _context.InventoryLevelTable.Add(inventoryLevel);
                    _context.SaveChanges(); // Save each entry to get the generated InventoryId
                    productToInventoryIdMap[productId] = inventoryLevel.InventoryId; // Store the mapping
                }

                // Now generate and save alerts
                var lowStockProducts = dashboard.LowStockProducts;
                var overStockProducts = dashboard.OverStockProducts;

                foreach (var productId in lowStockProducts)
                {
                    var inventoryLevel = _context.InventoryLevelTable
                        .FirstOrDefault(il => il.ProductId == productId && il.DashboardId == dashboardEntity.DashboardId);
                    if (inventoryLevel != null)
                    {
                        var alert = new InventoryAlertsTable
                        {
                            InventoryId = inventoryLevel.InventoryId,
                            ProductId = productId,
                            AlertType = "LS",
                            AlertDate = now
                        };
                        _context.InventoryAlertsTable.Add(alert);
                    }
                }

                foreach (var productId in overStockProducts)
                {
                    var inventoryLevel = _context.InventoryLevelTable
                        .FirstOrDefault(il => il.ProductId == productId && il.DashboardId == dashboardEntity.DashboardId);
                    if (inventoryLevel != null)
                    {
                        var alert = new InventoryAlertsTable
                        {
                            InventoryId = inventoryLevel.InventoryId,
                            ProductId = productId,
                            AlertType = "OS",
                            AlertDate = now
                        };
                        _context.InventoryAlertsTable.Add(alert);
                    }
                }

                _context.SaveChanges();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine($"Error saving dashboard: {ex.Message}");
                throw;
            }
        }
    }

    public InventoryDashboardRDM? GetLatestDashboard()
    {
        var dashboardEntity = _context.DashboardTable
            .Where(t => t.TypeId == 2)
            .OrderByDescending(d => d.GeneratedDate)
            .FirstOrDefault();

        if (dashboardEntity == null)
            return null;

        var dashboard = new InventoryDashboardRDM(
            dashboardEntity.DashboardId,
            dashboardEntity.Name,
            dashboardEntity.RequestedStartDate,
            dashboardEntity.RequestedEndDate,
            dashboardEntity.ValidityDuration,
            dashboardEntity.GeneratedDate);


        // Get stock levels from InventoryLevelTable
        var inventoryLevels = _context.InventoryLevelTable
            .Where(i => i.DashboardId == dashboardEntity.DashboardId)
            .ToList();

        // Get thresholds from ProductThresholdTable
        var productThresholds = _context.ProductThresholdTable
            .ToDictionary(p => p.ProductId, p => p.Threshold);

        var stockLevels = new Dictionary<int, int>();
        var thresholds = new Dictionary<int, int>();
        var replenishmentStatuses = new Dictionary<int, bool>();

        foreach (var inventoryLevel in inventoryLevels)
        {
            stockLevels[inventoryLevel.ProductId] = inventoryLevel.StockLevel;
            thresholds[inventoryLevel.ProductId] = productThresholds.ContainsKey(inventoryLevel.ProductId)
                ? productThresholds[inventoryLevel.ProductId] ?? 100
                : 100;
            replenishmentStatuses[inventoryLevel.ProductId] = inventoryLevel.ReplenishmentStatus;
        }



        dashboard.UpdateStockThreshold(stockLevels, thresholds);
        dashboard.UpdateReplenishmentStatus();
        dashboard.GenerateAlerts(); // Ensure alerts are generated

        return dashboard;
    }

    public Dictionary<int, (int LowStockWeeks, int OverStockWeeks)> GetConsecutiveWeeklyAlerts()
    {
        var result = new Dictionary<int, (int LowStockWeeks, int OverStockWeeks)>();

        // Check the number of records in InventoryAlertsTable
        var alertCount = _context.InventoryAlertsTable.Count();

        // Query only InventoryAlertsTable
        var alerts = _context.InventoryAlertsTable
            .Select(a => new
            {
                a.ProductId,
                a.AlertType,
                a.AlertDate
            })
            .ToList();

        // Debug: Log the raw alerts
        Console.WriteLine("Raw Alerts:");
        foreach (var alert in alerts)
        {
            Console.WriteLine($"Product {alert.ProductId}, Type: '{alert.AlertType}' (Length: {alert.AlertType.Length}), Date: {alert.AlertDate}");
        }

        // Group by ProductId, AlertType, and Date to deduplicate same-day alerts
        var groupedAlerts = alerts
            .GroupBy(a => new { a.ProductId, a.AlertType, Date = a.AlertDate.Date })
            .Select(g => g.First())
            .GroupBy(a => new { a.ProductId, a.AlertType })
            .ToList();

        // Debug: Log the grouped alerts
        Console.WriteLine("Grouped Alerts:");
        foreach (var group in groupedAlerts)
        {
            Console.WriteLine($"Product {group.Key.ProductId}, Type: '{group.Key.AlertType}' (Length: {group.Key.AlertType.Length})");
            foreach (var alert in group)
            {
                Console.WriteLine($"  Date: {alert.AlertDate}");
            }
        }

        // Group by ProductId to process all alert types for each product
        var products = groupedAlerts.GroupBy(g => g.Key.ProductId);

        foreach (var productGroup in products)
        {
            int productId = productGroup.Key;
            int lowStockWeeks = 0;
            int overStockWeeks = 0;

            // Track week ranges
            var weekRanges = new Dictionary<string, (int Year, int Week)[]>();

            Console.WriteLine($"Processing Product {productId}");

            foreach (var group in productGroup)
            {
                var alertType = group.Key.AlertType?.Trim(); // Trim to remove any whitespace

                // Debug: Log the alertType
                Console.WriteLine($"Checking alertType for Product {productId}: '{alertType}' (Length: {alertType?.Length ?? 0})");

                // Convert to week numbers
                var weeklyFlags = group
                    .Select(a => new
                    {
                        Week = System.Globalization.CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(
                            a.AlertDate, System.Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday),
                        Year = a.AlertDate.Year
                    })
                    .Distinct()
                    .OrderBy(a => a.Year)
                    .ThenBy(a => a.Week)
                    .ToList();

                // Debug: Log the weekly flags
                Console.WriteLine($"Weekly Flags for Product {productId}, Type: {alertType}");
                foreach (var flag in weeklyFlags)
                {
                    Console.WriteLine($"  Year: {flag.Year}, Week: {flag.Week}");
                }

                int consecutiveWeeks = 1;
                int maxConsecutive = 0;

                if (weeklyFlags.Count == 1)
                {
                    maxConsecutive = 1;
                }
                else
                {
                    for (int i = 1; i < weeklyFlags.Count; i++)
                    {
                        var prev = weeklyFlags[i - 1];
                        var curr = weeklyFlags[i];

                        bool isNextWeek =
                            (curr.Year == prev.Year && curr.Week == prev.Week + 1) ||
                            (curr.Year == prev.Year + 1 && prev.Week >= 52 && curr.Week == 1);

                        if (isNextWeek)
                        {
                            consecutiveWeeks++;
                        }
                        else
                        {
                            maxConsecutive = Math.Max(maxConsecutive, consecutiveWeeks);
                            consecutiveWeeks = 1;
                        }
                    }
                    maxConsecutive = Math.Max(maxConsecutive, consecutiveWeeks);
                }

                if (weeklyFlags.Any())
                {
                    weekRanges[alertType] = new[]
                    {
                            (weeklyFlags.First().Year, weeklyFlags.First().Week),
                            (weeklyFlags.Last().Year, weeklyFlags.Last().Week)
                        };
                }

                if (alertType == "LS")
                {
                    lowStockWeeks = maxConsecutive;
                    Console.WriteLine($"Assigned lowStockWeeks = {lowStockWeeks} for Product {productId}");
                }
                else if (alertType == "OS")
                {
                    overStockWeeks = maxConsecutive;
                    Console.WriteLine($"Assigned overStockWeeks = {overStockWeeks} for Product {productId}");
                }
                else
                {
                    Console.WriteLine($"Unexpected alertType: '{alertType}' for Product {productId}");
                }

                Console.WriteLine($"Product {productId}, Type: {alertType}, Max Consecutive: {maxConsecutive}");
            }

            // Check for LS/OS overlap and adjust
            if (weekRanges.ContainsKey("LS") && weekRanges.ContainsKey("OS"))
            {
                var lsStart = weekRanges["LS"][0];
                var lsEnd = weekRanges["LS"][1];
                var osStart = weekRanges["OS"][0];
                var osEnd = weekRanges["OS"][1];

                int ToAbs((int Year, int Week) w) => w.Year * 52 + w.Week;
                int lsStartAbs = ToAbs(lsStart);
                int lsEndAbs = ToAbs(lsEnd);
                int osStartAbs = ToAbs(osStart);
                int osEndAbs = ToAbs(osEnd);

                bool isOverlapping = !(lsEndAbs + 1 < osStartAbs || osEndAbs + 1 < lsStartAbs);

                if (isOverlapping)
                {
                    Console.WriteLine($"Overlapping LS/OS alerts for Product {productId}. Keeping only the longer streak.");
                    if (lowStockWeeks >= overStockWeeks)
                    {
                        overStockWeeks = 0;
                    }
                    else
                    {
                        lowStockWeeks = 0;
                    }
                }
            }

            result[productId] = (lowStockWeeks, overStockWeeks);
            Console.WriteLine($"Final result[{productId}]: LS={lowStockWeeks}, OS={overStockWeeks}");
        }

        return result;
    }

    public Dictionary<int, int?> GetAllProductThresholds()
    {
        return _context.ProductThresholdTable
            .ToDictionary(p => p.ProductId, p => p.Threshold);
    }
}
