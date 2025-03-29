using CleanBrilliantCompany.DTO;

public class CostMapper
{
    private readonly CostSimulation? costSimulation;

    public CostMapper(CostSimulation? costSimulation = null)
    {
        this.costSimulation = costSimulation ?? throw new ArgumentNullException(nameof(costSimulation));
    }

    // 🔹 Fetch Product Manufacturers DTOs
    public List<ProductManufacturerDTO> GetAllManufacturers()
    {
        if (costSimulation == null || costSimulation.Manufacturers == null)
            return new List<ProductManufacturerDTO>();

        return costSimulation.Manufacturers
            .Select(m => new ProductManufacturerDTO
            {
                ManufacturerId = m.ManufacturerId,
                CompanyName = m.CompanyName,
                Address = m.Address,
                Email = m.Email
            })
            .ToList();
    }

    // 🔹 Fetch Product Batches DTOs
    public List<ProductBatchDTO> GetAllProductBatches()
    {
        if (costSimulation == null || costSimulation.ProductBatches == null)
        {
            Console.WriteLine("[DEBUG] No product batches found.");
            return new List<ProductBatchDTO>();
        }

        var batches = costSimulation.ProductBatches.ToList();
        Console.WriteLine($"[DEBUG] CostMapper: Retrieved {batches.Count} batches.");

        foreach (var batch in batches)
        {
            Console.WriteLine($"[DEBUG] CostMapper: PASSING BATCHES BatchCode {batch.BatchCode}, ProductId {batch.ProductId}, ManufacturerId {batch.ManufacturerId}");
        }

        return batches;
    }

    public List<ItemDTO> GetAllItems()
    {
        if (costSimulation == null || costSimulation.Items == null)
        {
            Console.WriteLine("[DEBUG] No items found.");
            return new List<ItemDTO>();
        }

        var items = costSimulation.Items.ToList();
        Console.WriteLine($"[DEBUG] CostMapper: Retrieved {items.Count} items.");

        foreach (var item in items)
        {
            Console.WriteLine($"[DEBUG] CostMapper: PASSING ITEMS ItemId {item.ItemId}, ProductId {item.ProductId}, BatchCode {item.BatchCode}, SalePrice {item.SalePrice}, ItemStatus {item.ItemStatus}");
        }

        return items;
    }
}