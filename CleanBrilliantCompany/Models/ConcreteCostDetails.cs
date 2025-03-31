using CleanBrilliantCompany.DTO;

public class ConcreteCostDetails : AbstractCostDetails
{
    private readonly List<ProductBatchDTO> productBatches;

    private readonly List<ItemDTO> items;  // ✅ Add items

    List<Alert> productAlerts = new List<Alert>();  // ✅ Separate alerts for product performance

    // ✅ Constructor
    public ConcreteCostDetails(IAlertService alertService, List<ProductBatchDTO> productBatches, List<ItemDTO> items)
        : base(alertService)
    {
        this.productBatches = productBatches;
        this.items = items;
    }

    public override object CheckBatchPerformance()
    {
        if (productBatches == null || !productBatches.Any() || items == null || !items.Any())
        {
            Console.WriteLine("[DEBUG] No data found for checking batches!");
            return new { alerts = new List<Alert>() }; // ✅ Return empty alerts list if no data
        }

        foreach (var batch in productBatches)
        {
            var relatedItems = items.Where(i => i.BatchCode == batch.BatchCode).ToList();
            int totalSold = relatedItems.Count(i => i.ItemStatus == "Sold");
            int initialQuantity = batch.BatchQuantity;
            int totalRemaining = initialQuantity - totalSold;  // Remaining items in batch
            decimal totalSalePrice = relatedItems.Sum(i => i.SalePrice); // Total revenue from sales
            decimal batchCost = batch.BatchPrice;  // ✅ Corrected batch cost

            Console.WriteLine($"[DEBUG] Batch {batch.BatchCode} - Initial Quantity: {initialQuantity}, Sold: {totalSold}, Remaining: {totalRemaining}, Sales: {totalSalePrice:C}");

            if (totalRemaining == 0)  // ✅ Batch is fully sold out
            {
                decimal profitOrLoss = totalSalePrice - batchCost;
                string result = profitOrLoss >= 0 ? "Profit" : "Loss";

                Console.WriteLine($"[DEBUG] Batch {batch.BatchCode} - {result}: {profitOrLoss:C}");

                if (result == "Loss")  // ✅ Only generate alert for Loss
                {
                    var alert = alertService.GenerateBudgetAlert(
                        $"⚠️ Batch {batch.BatchCode} is fully sold out & suffered a devastating loss! Loss: {profitOrLoss:C}. " +
                        $"Initial Quantity: {initialQuantity}, Sold: {totalSold}, Remaining: {totalRemaining}."
                    );
                    alerts.Add(alert);
                }
            }
        }

        return new { alerts = GetAlerts() }; // ✅ Ensure the method returns an object
    }

    public override object CheckProductPerformance(int productId)
    {
        var filteredBatches = productBatches.Where(b => b.ProductId == productId).ToList();
        var filteredItems = items.Where(i => filteredBatches.Any(b => b.BatchCode == i.BatchCode)).ToList();
        
        if (!filteredBatches.Any()) {
            Console.WriteLine($"[DEBUG] No batches found for Product ID: {productId}");
            return new { alerts = new List<Alert>() }; // ✅ No batches at all, return empty alert list
        }

        foreach (var batch in filteredBatches)
        {
            var relatedItems = filteredItems.Where(i => i.BatchCode == batch.BatchCode).ToList();
            int totalSold = relatedItems.Count(i => i.ItemStatus == "Sold");
            int initialQuantity = batch.BatchQuantity;
            int totalRemaining = initialQuantity - totalSold;  
            decimal totalSalePrice = relatedItems.Sum(i => i.SalePrice); 
            decimal batchCost = batch.BatchPrice;

            Console.WriteLine($"[DEBUG] Batch {batch.BatchCode} - Initial Quantity: {initialQuantity}, Sold: {totalSold}, Remaining: {totalRemaining}, Batch Price {batchCost}, Sales: {totalSalePrice:C}");

            string profitOrLossText = "";
            if (totalRemaining == 0) 
            {
                decimal profitOrLoss = totalSalePrice - batchCost;
                string result = profitOrLoss >= 0 ? "Profit" : "Loss";
                profitOrLossText = $", {result}: {profitOrLoss:C}";
            }

            decimal avgSalePricePerUnit = totalSold > 0 ? totalSalePrice / totalSold : 0;

            var alert = alertService.GenerateBudgetAlert(
                $"📊 Batch {batch.BatchCode} Summary: " +
                $"Initial Quantity: {initialQuantity}, Sold: {totalSold}, Remaining: {totalRemaining}, " +
                $"Initial Batch Cost: {batchCost:C}, Current Batch Sales: {totalSalePrice:C}{profitOrLossText}, "+
                $"Avg Sale Price per Unit: {avgSalePricePerUnit:C}."
            );
            productAlerts.Add(alert);
        }

        // ✅ If there were no transactions but batches exist, still return alerts.
        if (!productAlerts.Any())
        {
            Console.WriteLine("[DEBUG] No transactions for product, returning default alert.");
            var defaultAlert = alertService.GenerateBudgetAlert(
                $"📊 No transactions recorded for Product ID {productId}. " +
                $"Batches exist, but no sales have been made."
            );
            productAlerts.Add(defaultAlert);
        }

        return new { alerts = productAlerts };  
    }
       

    public override object GetBatchBudgetSummary()
    {
        // CheckBatchBudget();  // ✅ Triggers the alert logic
        CheckBatchPerformance();
        return new { alerts = GetAlerts() };
    }
}