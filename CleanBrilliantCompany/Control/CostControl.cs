public class CostControl 
{

    private readonly ApplicationDbContext dbContext;

    private List<CostDashboardRdm> dashboards; 
    private CostMapper costMapper; 
    private readonly ILogger<CostDashboardRdm> logger;  // Inject Logger

    private readonly IVisualizationService visualizationService;  // Inject Visualization Service

    private readonly IAlertService alertService;  // Inject Alert Service

    private readonly DashboardFactory dashboardFactory;  // ✅ Use Factory


    public CostControl(CostMapper costMapper, ILogger<CostDashboardRdm> logger,IVisualizationService visualizationService, IAlertService alertService,
    ApplicationDbContext dbContext )
    {
        this.dbContext = dbContext;
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

    logger.LogInformation("[DEBUG] Attempting to load dashboards from database...");
    
    var dashboardEntity = dbContext.Dashboards
        .Where(d => d.TypeId == 4)
        .OrderByDescending(d => d.RequestedStartDate)
        .FirstOrDefault();

    CostDashboardRdm costDashboard;

    if (dashboardEntity == null)
    {
        logger.LogWarning("⚠ No dashboard found in database. Creating a new one.");

        var newDashboardDto = new DashboardDTO
        {
            Name = "New Cost Dashboard Generated",
            RequestedStartDate = DateTime.Now,
            RequestedEndDate = DateTime.Now,
            GeneratedDate = DateTime.Now,
            ValidityDuration = 1,
            TypeId = 4
        };

        var newEntity = DashboardMapper.ToEntity(newDashboardDto);
        dbContext.Dashboards.Add(newEntity);
        dbContext.SaveChanges();

        // ✅ Use factory for new dashboards
        costDashboard = (CostDashboardRdm)dashboardFactory.CreateDashboard(
            "Cost",
            newDashboardDto.Name,
            newDashboardDto.RequestedStartDate,
            newDashboardDto.RequestedEndDate,
            newDashboardDto.TypeId
        );
    }
    else
    {
        // ✅ Use factory for loading existing dashboards too
        var dto = DashboardMapper.ToDTO(dashboardEntity);

        costDashboard = (CostDashboardRdm)dashboardFactory.CreateDashboard(
            "Cost",
            dto.Name,
            dto.RequestedStartDate,
            dto.RequestedEndDate,
            dto.TypeId
        );
    }

    var manufacturers = costMapper.GetAllManufacturers();
    var productBatches = costMapper.GetAllProductBatches();
    var item = costMapper.GetAllItems();

    costDashboard.ProcessManufacturers(manufacturers);
    costDashboard.ProcessProductBatches(productBatches);
    costDashboard.ProcessItems(item);

    dashboards.Add(costDashboard);
    logger.LogInformation($"✅ Loaded dashboard: {costDashboard.Name}");
}

    // 🔹 Method to Retrieve the Latest Dashboard
    public CostDashboardRdm? GetLatestDashboard()
    {
        return dashboards.OrderByDescending(d => d.StartDate).FirstOrDefault();
    }

    public bool GenerateNewDashboardIfOutdated()
    {
        logger.LogInformation("[DEBUG] Checking for outdated dashboard...");

        var existingDashboard = dbContext.Dashboards
            .Where(d => d.TypeId == 4)
            .OrderByDescending(d => d.GeneratedDate)
            .FirstOrDefault();

        if (existingDashboard != null && existingDashboard.GeneratedDate.Date == DateTime.Today)
        {
            logger.LogInformation("✅ Existing dashboard is already up-to-date.");
            return false; // nothing changed
        }

        if (existingDashboard != null)
        {
            logger.LogInformation("🗑 Deleting outdated dashboard...");
            dbContext.Dashboards.Remove(existingDashboard);
            dbContext.SaveChanges();
        }

        var newDashboardDto = new DashboardDTO
        {
            Name = "New Cost Dashboard Generated",
            RequestedStartDate = DateTime.Now,
            RequestedEndDate = DateTime.Now.AddMonths(1),
            GeneratedDate = DateTime.Now,
            ValidityDuration = 1,
            TypeId = 4
        };

        var newEntity = DashboardMapper.ToEntity(newDashboardDto);
        dbContext.Dashboards.Add(newEntity);
        dbContext.SaveChanges();

        var costDashboard = (CostDashboardRdm)dashboardFactory.CreateDashboard(
            "Cost",
            newDashboardDto.Name,
            newDashboardDto.RequestedStartDate,
            newDashboardDto.RequestedEndDate,
            newDashboardDto.TypeId
        );

        costDashboard.ProcessManufacturers(costMapper.GetAllManufacturers());
        costDashboard.ProcessProductBatches(costMapper.GetAllProductBatches());
        costDashboard.ProcessItems(costMapper.GetAllItems());

        dashboards.Add(costDashboard);

        logger.LogInformation($"✅ Created new dashboard: {costDashboard.Name}");
        return true; // new dashboard created
    }
}