using CleanBrilliantCompany.DatabaseEntities;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public DbSet<DashboardTable> Dashboards { get; set; }
    public DbSet<AgingAnalyticsDetailsTable> AgingAnalyticsDetails { get; set; }

    // Inventory
    public DbSet<DashboardTable> DashboardTable { get; set; }
    public DbSet<DashboardTypeTable> DashboardTableType { get; set; }
    public DbSet<AlertTypeTable> AlertTypeTable { get; set; }
    public DbSet<InventoryLevelTable> InventoryLevelTable { get; set; }
    public DbSet<InventoryAlertsTable> InventoryAlertsTable { get; set; }
    public DbSet<ProductThresholdTable> ProductThresholdTable { get; set; }
    //public DbSet<ProductTable> ProductTable { get; set; } // Added for simulation

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Map entity classes to actual database tables (if necessary)
        modelBuilder.Entity<DashboardTable>().ToTable("Dashboard");
        modelBuilder.Entity<DashboardTypeTable>().ToTable("DashboardType");
        modelBuilder.Entity<AgingAnalyticsDetailsTable>().ToTable("AgingAnalyticsDetails");
        // Inventory
        modelBuilder.Entity<AlertTypeTable>().ToTable("InventoryAlertType");
        modelBuilder.Entity<InventoryLevelTable>().ToTable("InventoryLevel");
        modelBuilder.Entity<InventoryAlertsTable>().ToTable("InventoryAlerts");
        //modelBuilder.Entity<ProductTable>().ToTable("Product");
        modelBuilder.Entity<ProductThresholdTable>().ToTable("ProductThreshold");

        // Configure DashboardTable entity and set the primary key 
        modelBuilder.Entity<DashboardTable>()
            .HasKey(d => d.DashboardId);  // Set DashboardId as the primary key 

        // Configure AgingAnalyticsDetailsTable entity and set the primary key
        modelBuilder.Entity<AgingAnalyticsDetailsTable>()
            .HasKey(a => a.AnalyticsId);  // Set AnalyticsId as the primary key

        modelBuilder.Entity<DashboardTypeTable>()
            .HasKey(t => t.TypeId);

        modelBuilder.Entity<AlertTypeTable>()
            .HasKey(a => a.AlertType);

        modelBuilder.Entity<InventoryLevelTable>()
            .HasKey(i => i.InventoryId);

        modelBuilder.Entity<InventoryAlertsTable>()
            .HasKey(a => a.AlertId);

        //modelBuilder.Entity<ProductTable>()
        //    .HasKey(p => p.productId);



        base.OnModelCreating(modelBuilder);
    }
}
