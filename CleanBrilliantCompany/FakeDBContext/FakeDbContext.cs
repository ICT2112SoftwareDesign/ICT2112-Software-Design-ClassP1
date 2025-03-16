public class FakeDbContext
{
    public List<DashboardDTO> Dashboards { get; set; }
    public List<AgingAnalyticsDetailsDTO> AgingAnalyticsDetails { get; set; }

    public FakeDbContext()
    {
        // 🔹 Fake Dashboards
        Dashboards = new List<DashboardDTO>
        {
            new DashboardDTO
            {
                DashboardId = 1,
                Name = "Aging Dashboard 1",
                RequestedStartDate = DateTime.Now.AddMonths(-6),
                RequestedEndDate = DateTime.Now,
                GeneratedDate = DateTime.Now.AddDays(-1),
                ValidityDuration = 180,
                Type = 1
            }
        };

        // 🔹 Fake Aging Analytics
        AgingAnalyticsDetails = new List<AgingAnalyticsDetailsDTO>
        {
            new AgingAnalyticsDetailsDTO
            {
                AnalyticsId = 1001,
                BatchCode = 101,
                DashboardId = 1,
                DaysInStorage = 120,
                IsExpired = false,
                RemainingDays = 60,
                TurnOverRate = 75.5,
                DeadStockPercentage = 24.5
            },
            new AgingAnalyticsDetailsDTO
            {
                AnalyticsId = 1002,
                BatchCode = 102,
                DashboardId = 1,
                DaysInStorage = 90,
                IsExpired = false,
                RemainingDays = 30,
                TurnOverRate = 55.0,
                DeadStockPercentage = 45.0
            }
        };
    }
}
