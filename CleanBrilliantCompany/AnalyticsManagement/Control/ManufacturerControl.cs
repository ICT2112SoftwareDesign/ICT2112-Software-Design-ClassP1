using CleanBrilliantCompany.Interfaces;

public class ManufacturerControl
{
    private Dashboard manufacturerDashboard;
    private readonly ManufacturerRepository ManufacturerMapper;
    private readonly IReorder _Ireorder;
    private readonly IManufacturer _Imanufacturer;

    public ManufacturerControl(ManufacturerRepository ManufacturerMapper, IReorder _Ireorder, IManufacturer _Imanufacturer)
    {
        this.ManufacturerMapper = ManufacturerMapper;
        this._Ireorder = _Ireorder;
        this._Imanufacturer =  _Imanufacturer;
        LoadDashboard();
    }

    // Load the latest manufacturer dashboard
    private void LoadDashboard()
    {
        var dashboardDto = ManufacturerMapper.GetLatestManufacturerDashboard();
        if (dashboardDto == null)
        {
            Console.WriteLine("⚠ No manufacturer dashboard found.");
            return;
        }

        // Check if the dates are null, and provide default values if needed
        DateTime requestedStartDate = dashboardDto.RequestedStartDate;
        DateTime requestedEndDate = dashboardDto.RequestedEndDate;

        // Create the dashboard object using the factory method
        manufacturerDashboard = DashboardFactory.createDashboard(dashboardDto) as ManufacturerDashboardRdm;

        // Fetch the manufacturer metrics and assign it to the dashboard
        var metricsList = ManufacturerMapper.GetManufacturerMetrics(dashboardDto.DashboardId);

        if (manufacturerDashboard is ManufacturerDashboardRdm manufacturerDashboardRdm)
        {
            manufacturerDashboardRdm.Metrics = metricsList;

            manufacturerDashboardRdm.MetricDetailsList = ManufacturerMapper
            .GetManufacturerMetrics(dashboardDto.DashboardId)
            .Select(m => {
            // Fetch the manufacturer details
            var manufacturerDetails = _Imanufacturer.getManufacturerDetails(m.ManufacturerId);
            var companyName = manufacturerDetails.retrieveProductManufacturerInfo()["CompanyName"].ToString();

            // Replace ManufacturerId with the company name directly
            return new ManufacturerMetricDetails(
                companyName,  // Use the company name instead of ID
                m.DeliveryRate,
                m.DefectRate,
                m.DependencyRate,
                m.RiskFlag
            );
        }).ToList();
        }
        else
        {
            Console.WriteLine("⚠ Manufacturer dashboard could not be created.");
        }
    }

    // Method to retrieve the latest dashboard
    public ManufacturerDashboardRdm? GetLatestDashboard()
    {
        return manufacturerDashboard as ManufacturerDashboardRdm;
    }

    // Method to generate a new manufacturer dashboard and calculate/save metrics
    public void GenerateNewDashboard(DashboardDTO dto)
    {
        // Set the type for the dashboard
        dto.Type = 3; // 3 corresponds to Manufacturer Dashboard Type ID

        // Create the new dashboard
        var dashboard = DashboardFactory.createDashboard(dto);

        // Get reorders from IReorder
        var reorders = IReorder.GetAllReorderDetails(); 

        // Filter the reorders by the requested start and end date
        var filteredOrders = reorders
            .Where(b => b.ExpectedDeliveryDate >= dashboard.RequestedStartDate && b.ExpectedDeliveryDate <= dashboard.RequestedEndDate)
            .ToList();

        // Populate the analytics section of the dashboard
        (dashboard as ManufacturerDashboardRdm).populateMetrics(filteredOrders);

        // Save the newly generated dashboard and analytics
        ManufacturerMapper.saveDashboardandMetrics(dashboard as ManufacturerDashboardRdm);
    }

    public string GenerateReport()
    {
        var dashboard = GetLatestDashboard();

        if (dashboard == null)
        {
            Console.WriteLine("❌ Manufacturer dashboard is null.");
            return "<p>No data found for manufacturer dashboard.</p>";
        }

        var report = new System.Text.StringBuilder();

        report.AppendLine($"<h1>Manufacturer Report - {dashboard.Name}</h1>");
        report.AppendLine($"<p>Generated: {dashboard.GeneratedDate}</p>");
        report.AppendLine("<hr/>");

        report.AppendLine("<h2>Manufacturer Metrics</h2>");
        report.AppendLine("<table border='1' cellpadding='6' cellspacing='0' style='border-collapse: collapse;'>");
        report.AppendLine("<thead><tr>");
        report.AppendLine("<th>Manufacturer ID</th>");
        report.AppendLine("<th>Delivery Rate</th>");
        report.AppendLine("<th>Defect Rate</th>");
        report.AppendLine("<th>Dependency Rate</th>");
        report.AppendLine("<th>Risk Flag</th>");
        report.AppendLine("</tr></thead>");
        report.AppendLine("<tbody>");

        foreach (var detail in dashboard.MetricDetailsList)
        {
            report.AppendLine("<tr>");
            report.AppendLine($"<td>{detail.ManufacturerId}</td>");
            report.AppendLine($"<td>{detail.DeliveryRate:P}</td>");
            report.AppendLine($"<td>{detail.DefectRate:P}</td>");
            report.AppendLine($"<td>{detail.DependencyRate:P}</td>");
            report.AppendLine($"<td>{(detail.RiskFlag ? "High Risk" : "Low Risk")}</td>");
            report.AppendLine("</tr>");
        }

        report.AppendLine("</tbody></table>");
        return report.ToString();
    }

}
