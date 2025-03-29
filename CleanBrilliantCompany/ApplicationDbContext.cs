using CleanBrilliantCompany.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using CleanBrilliantCompany.DatabaseEntities;
public class ApplicationDbContext : DbContext
{
    public DbSet<DashboardTable> Dashboards { get; set; }
    public DbSet<ItemTable> Items { get; set; }
    public DbSet<ProductTable> Product { get; set; }
    public DbSet<ProductBatchTable> ProductBatch { get; set; }

    public DbSet<ManufacturerTable> Manufacturers { get; set; }

    public DbSet<ManufacturerAnalyticsTable> ManufacturerAnalytics { get; set; }

    public DbSet<ManufacturerCostAnalyticsTable> ManufacturerCostAnalytics { get; set; }

    public DbSet<DashboardBatchSummaryTable> DashboardBatchSummary { get; set; }

    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DashboardTable>().ToTable("Dashboard");
        modelBuilder.Entity<DashboardTable>()
            .HasKey(d => d.DashboardId);

        modelBuilder.Entity<ItemTable>().ToTable("Item");
        modelBuilder.Entity<ManufacturerTable>().ToTable("ProductManufacturer");

        modelBuilder.Entity<ProductBatchTable>();

        modelBuilder.Entity<ManufacturerAnalyticsTable>().ToTable("ManufacturerAnalytics").HasKey(m => m.ManufacturerAnalyticsId);
        modelBuilder.Entity<ManufacturerCostAnalyticsTable>().ToTable("ManufacturerCostAnalytics").HasKey(c => c.CostAnalyticsId);;
     

        var decimalToDoubleConverter = new ValueConverter<decimal, double>(
            v => (double)v,               // Convert decimal to double when saving to the DB.
            v => Convert.ToDecimal(v)     // Convert double to decimal when reading from the DB.
        );

        modelBuilder.Entity<ManufacturerCostAnalyticsTable>()
            .Property(m => m.AvgBatchCost)
            .HasConversion(decimalToDoubleConverter);


        modelBuilder.Entity<DashboardBatchSummaryTable>().ToTable("DashboardBatchSummary").HasKey(s => s.BatchSummaryId);;

        base.OnModelCreating(modelBuilder);
    }
}