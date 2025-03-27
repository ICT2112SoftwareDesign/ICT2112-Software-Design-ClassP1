using CleanBrilliantCompany.DTO;
using Microsoft.EntityFrameworkCore;
public class ApplicationDbContext : DbContext
{
    public DbSet<DashboardTable> Dashboards { get; set; }
    public DbSet<ItemTable> Items { get; set; }

    public DbSet<ProductTable> Product { get; set; }

    public DbSet<ProductBatchTable> ProductBatch { get; set; }
    public DbSet<ManufacturerTable> Manufacturers { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DashboardTable>().ToTable("Dashboard");
        modelBuilder.Entity<DashboardTable>()
            .HasKey(d => d.DashboardId);

        modelBuilder.Entity<ItemTable>().ToTable("Item");
        modelBuilder.Entity<ManufacturerTable>().ToTable("ProductManufacturer");

        modelBuilder.Entity<ProductBatchTable>();

        base.OnModelCreating(modelBuilder);
    }
}