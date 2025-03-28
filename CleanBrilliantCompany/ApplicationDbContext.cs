using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    // DbSets for the tables related to the Manufacturer Dashboard
    public DbSet<DashboardTable> Dashboards { get; set; }
    public DbSet<ManufacturerMetricsTable> ManufacturerMetrics { get; set; }
    public DbSet<ProductManufacturerTable> ProductManufacturers { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Map entity classes to actual database tables (if necessary)
        modelBuilder.Entity<DashboardTable>().ToTable("Dashboard");
        modelBuilder.Entity<ManufacturerMetricsTable>().ToTable("ManufacturerMetrics");
        modelBuilder.Entity<ProductManufacturerTable>().ToTable("ProductManufacturer");

        // Configure DashboardTable entity and set the primary key 
        modelBuilder.Entity<DashboardTable>()
            .HasKey(d => d.DashboardId);  // Set DashboardId as the primary key 

        // Configure ManufacturerMetricsTable entity and set the primary key
        modelBuilder.Entity<ManufacturerMetricsTable>()
            .HasKey(m => m.MetricId);  // Set MetricId as the primary key

        // Configure ProductManufacturerTable entity and set the primary key
        modelBuilder.Entity<ProductManufacturerTable>()
            .HasKey(p => p.ManufacturerId);  // Set ManufacturerId as the primary key

        base.OnModelCreating(modelBuilder);
    }

    
}
