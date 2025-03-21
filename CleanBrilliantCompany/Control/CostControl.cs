public class CostControl 
{
    private List<CostDashboardRdm> dashboards; 
    private CostMapper costMapper; 
    private readonly ILogger<CostDashboardRdm> logger;  // Inject Logger

    private readonly IVisualizationService visualizationService;  // Inject Visualization Service

    private readonly IAlertService alertService;  // Inject Alert Service

    private readonly DashboardFactory dashboardFactory;  // ✅ Use Factory



    public CostControl(CostMapper costMapper, ILogger<CostDashboardRdm> logger,IVisualizationService visualizationService, IAlertService alertService)
    {
        this.costMapper = costMapper;
        this.logger = logger;
        this.visualizationService = visualizationService;
        this.alertService = alertService;
        this.dashboards = new List<CostDashboardRdm>();
        this.dashboardFactory = new DashboardFactory(logger, visualizationService,alertService); 
        LoadDashboards(); 
    }

    // 🔹 Load dashboards from the database (or fake DB)
    private void LoadDashboards()
    {
        var dashboardDto = costMapper.GetLatestCostDashboard();
        if (dashboardDto == null)
        {
            logger.LogWarning("⚠ No dashboard found in CostControl.");
            return; // Prevent null reference
        }


        // Retrieving Data from the “Database” (CostMapper)

        var manufacturers = costMapper.GetAllManufacturers();
        var productBatches = costMapper.GetAllProductBatches();

        // var newDashboardDto = new DashboardDTO
        // {
        //     Name = "New Dashboard Generated 2.0",
        //     RequestedStartDate = DateTime.Now,
        //     RequestedEndDate = DateTime.Now.AddMonths(5),
        //     ValidityDuration = 50
        // };

        // Factory Method of Creating a New Dashboard

        var newDashboard = dashboardFactory.CreateDashboard("Cost", "New Dashboard", DateTime.Now, DateTime.Now.AddMonths(1), 6);
        var costDashboard = (CostDashboardRdm)newDashboard;


        // Passing Retrieved Data to CostDashboardRdm
        Console.WriteLine($"[DEBUG] CostControl: Passing {productBatches.Count} batches to CostDashboardRdm");
        costDashboard.ProcessManufacturers(manufacturers);
        costDashboard.ProcessProductBatches(productBatches);


        // This loads the dashboard from the database
        // var costDashboard = new CostDashboardRdm(dashboardDto, manufacturers, productBatches,logger,visualizationService);

        // This creates a new dashboard using the DTO
        // var costDashboard = new CostDashboardRdm(newDashboardDto, manufacturers, productBatches,logger,visualizationService);
    

        dashboards.Add(costDashboard);

        logger.LogInformation($"✅ Loaded dashboard: {dashboardDto.Name}");
    }

    // 🔹 Method to Retrieve the Latest Dashboard
    public CostDashboardRdm? GetLatestDashboard()
    {
        return dashboards.OrderByDescending(d => d.StartDate).FirstOrDefault();
    }
}