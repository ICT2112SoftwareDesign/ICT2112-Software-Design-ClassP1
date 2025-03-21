using CleanBrilliantCompany.DTO;

public class CostMapper
{
    private readonly CostSimulation? costSimulation;

    public CostMapper(CostSimulation? costSimulation = null)
    {
        this.costSimulation = costSimulation ?? throw new ArgumentNullException(nameof(costSimulation));
    }

    // 🔹 Fetch Dashboard DTO
    public DashboardDTO GetLatestCostDashboard()
    {
        if (costSimulation == null || costSimulation.Dashboards == null || !costSimulation.Dashboards.Any())
        {
            Console.WriteLine("⚠ No existing dashboards found. Creating a default dashboard.");
            return new DashboardDTO
            {
                DashboardId = 999, 
                Name = "Default Dashboard",
                RequestedStartDate = DateTime.Now.AddMonths(-1),
                RequestedEndDate = DateTime.Now,
                GeneratedDate = DateTime.Now,
                ValidityDuration = 30,
                Type = 0
            };
        }

        var latest = costSimulation.Dashboards.OrderByDescending(d => d.GeneratedDate).FirstOrDefault();
        Console.WriteLine($"✅ Found existing dashboard: {latest.Name} (ID: {latest.DashboardId})");
        return latest;
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
            Console.WriteLine($"[DEBUG] CostMapper: BatchCode {batch.BatchCode}, ProductId {batch.ProductId}, ManufacturerId {batch.ManufacturerId}");
        }

        return batches;
    }
        

}