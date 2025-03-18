public class AgingMapper : AgingRepo
{
    //private readonly AppDbContext? realDbContext;
    private readonly FakeDbContext? fakeDbContext;

    // 🔹 Constructor supports BOTH Fake & Real DB
    public AgingMapper(FakeDbContext? fakeDbContext = null)
    {
        //this.realDbContext = realDbContext;
        this.fakeDbContext = fakeDbContext ?? throw new ArgumentNullException(nameof(fakeDbContext)); 
    }

    // 🔹 Fetch Dashboard DTO
    public DashboardDTO? GetLatestAgingDashboard()
    {
        //Ensure `fakeDbContext` is non-null before accessing `Dashboards`
        if (fakeDbContext == null || fakeDbContext.Dashboards == null)
        return null; // Return null if no dashboards exist

        return (fakeDbContext.Dashboards) 
            .OrderByDescending(d => d.GeneratedDate)
            .Select(d => new DashboardDTO
            {
                DashboardId = d.DashboardId,
                Name = d.Name,
                RequestedStartDate = d.RequestedStartDate,
                RequestedEndDate = d.RequestedEndDate,
                GeneratedDate = d.GeneratedDate,
                ValidityDuration = d.ValidityDuration,
                Type = d.Type
            })
            .FirstOrDefault();
    }

    // 🔹 Fetch Aging Analytics DTOs
    public List<AgingAnalyticsDetailsDTO> GetAgingAnalytics(int dashboardId)
    {
        if (fakeDbContext == null || fakeDbContext.AgingAnalyticsDetails == null)
            return new List<AgingAnalyticsDetailsDTO>(); // Return empty list if no data


        return (fakeDbContext.AgingAnalyticsDetails) 
            .Where(a => a.DashboardId == dashboardId)
            .Select(a => new AgingAnalyticsDetailsDTO
            {
                AnalyticsId = a.AnalyticsId,
                BatchCode = a.BatchCode,
                DashboardId = a.DashboardId,
                DaysInStorage = a.DaysInStorage,
                IsExpired = a.IsExpired,
                RemainingDays = a.RemainingDays,
                TurnOverRate = a.TurnOverRate,
                DeadStockPercentage = a.DeadStockPercentage
            })
            .ToList();
    }
}
