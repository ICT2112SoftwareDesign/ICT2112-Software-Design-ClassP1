using CleanBrilliantCompany.DTO;

public class DashboardFactory
{
    private readonly ILogger<CostDashboardRdm> logger;
    private readonly IVisualizationService visualizationService;
    private readonly IAlertService alertService;

    public DashboardFactory(ILogger<CostDashboardRdm> logger, IVisualizationService visualizationService, IAlertService alertService)
    {
        this.logger = logger;
        this.visualizationService = visualizationService;
        this.alertService = alertService;
    }

    public Dashboard CreateDashboard(string type, string name, DateTime startDate, DateTime endDate, int validityDuration)
    {
        var newDashboardDto = new DashboardDTO
        {
            Name = name,
            RequestedStartDate = startDate,
            RequestedEndDate = endDate,
            ValidityDuration = validityDuration
        };

        return type switch
        {
            // "Aging" => new AgingDashboardRdm(newDashboardDto, new List<ProductAgeDTO>(), new List<ProductBatchDTO>(), logger, visualizationService),
            // "Inventory" => new InventoryDashboardRdm(newDashboardDto, new List<InventoryDTO>(), logger),
            "Cost" => new CostDashboardRdm(newDashboardDto, new List<ProductManufacturerDTO>(), new List<ProductBatchDTO>(), new List<ItemDTO>(), logger, visualizationService, alertService),
            _ => throw new ArgumentException("Invalid dashboard type")
        };

        // return new CostDashboardRdm(newDashboardDto, new List<ProductManufacturerDTO>(), new List<ProductBatchDTO>(), logger, visualizationService);
    }
}