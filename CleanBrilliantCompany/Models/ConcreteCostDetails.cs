using CleanBrilliantCompany.DTO;

public class ConcreteCostDetails : AbstractCostDetails
{
    private readonly List<ProductBatchDTO> productBatches;
    private const decimal COST_THRESHOLD = 500m;  // Set your own limit

    // ✅ Constructor
    public ConcreteCostDetails(IAlertService alertService, List<ProductBatchDTO> productBatches)
        : base(alertService)
    {
        this.productBatches = productBatches;
    }

    private void CheckBatchBudget()
    {
        if (productBatches == null || !productBatches.Any())
        {
            Console.WriteLine("[DEBUG] No product batches found!");  // ✅ Check if list is empty
            return;
        }

        Console.WriteLine($"[DEBUG] Checking budget for {productBatches.Count} batches");  // ✅ How many batches?

        foreach (var batch in productBatches)
        {
            decimal totalCost = batch.BatchPrice;
            Console.WriteLine($"[DEBUG] Batch {batch.BatchCode}: Cost={totalCost}");

            if (totalCost > COST_THRESHOLD)  // Custom alert logic
            {
                decimal exceededAmount = totalCost - COST_THRESHOLD;
                decimal exceededPercentage = (exceededAmount / COST_THRESHOLD) * 100;

                Console.WriteLine($"[DEBUG] Batch {batch.BatchCode} exceeded by {exceededAmount:C} ({exceededPercentage:F2}%)");

                // ✅ Create alert
                var alert = alertService.GenerateBudgetAlert(
                    $"⚠ Budget Alert: Batch {batch.BatchCode} exceeded the threshold by {exceededAmount:C} ({exceededPercentage:F2}%)"
                );

                alerts.Add(alert);  // ✅ Store the alert
            }
        }
    }

    public override object GetBatchBudgetSummary()
    {
        CheckBatchBudget();  // ✅ Triggers the alert logic
        return new { alerts = GetAlerts() };
    }

    public override float CalculateTotalCost()
    {
        float totalCost = (float)productBatches.Sum(batch => batch.BatchPrice);
        BudgetUsed = totalCost;
        return totalCost;
    }

    public override float CalculateSavings(int manufacturerId)
    {
        var manufacturerBatches = productBatches.Where(b => b.ManufacturerId == manufacturerId);
        return (float)manufacturerBatches.Sum(batch => batch.BatchPrice * 0.1m); // Example 10% savings
    }

    public override int GetCheapestManufacturer(int productId)
    {
        var cheapestBatch = productBatches
            .Where(batch => batch.ProductId == productId)
            .OrderBy(batch => batch.BatchPrice)
            .FirstOrDefault();

        return cheapestBatch?.ManufacturerId ?? -1;
    }
}