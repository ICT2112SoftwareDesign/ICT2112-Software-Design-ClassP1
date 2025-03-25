using CleanBrilliantCompany.Models;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{

    public DbSet<DashboardTable> Dashboards { get; set; }
    public DbSet<AgingAnalyticsDetailsTable> AgingAnalyticsDetails { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Report> Reports { get; set; }
    public DbSet<ReportLog> ReportLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Map entity classes to actual database tables (if necessary)
        modelBuilder.Entity<DashboardTable>().ToTable("Dashboard");
        modelBuilder.Entity<AgingAnalyticsDetailsTable>().ToTable("AgingAnalyticsDetails");

        // Configure DashboardTable entity and set the primary key 
        modelBuilder.Entity<DashboardTable>()
            .HasKey(d => d.DashboardId);  // Set DashboardId as the primary key 

        // Configure AgingAnalyticsDetailsTable entity and set the primary key
        modelBuilder.Entity<AgingAnalyticsDetailsTable>()
            .HasKey(a => a.AnalyticsId);  // Set AnalyticsId as the primary key

        // For Report and AnalyticsReportLog
        modelBuilder.Entity<Report>().ToTable("Report");
        modelBuilder.Entity<ReportLog>()
        .ToTable("AnalyticsReportLog"); // rename table mapping

        base.OnModelCreating(modelBuilder);
    }
}
