using CleanBrilliantCompany.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanBrilliantCompany.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<DashboardTable> DashboardTable { get; set; }
        public DbSet<DashboardTypeTable> DashboardTableType { get; set; }
        public DbSet<AlertTypeTable> AlertTypeTable { get; set; }
        public DbSet<InventoryLevelTable> InventoryLevelTable { get; set; }
        public DbSet<InventoryAlertsTable> InventoryAlertsTable { get; set; }
        public DbSet<ProductThresholdTable> ProductThresholdTable { get; set; }
        public DbSet<ProductTable> ProductTable { get; set; } // Added for simulation



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define primary key
            modelBuilder.Entity<DashboardTable>()
                .HasKey(d => d.DashboardId);

            modelBuilder.Entity<DashboardTypeTable>()
                .HasKey(t => t.TypeId);

            modelBuilder.Entity<AlertTypeTable>()
                .HasKey(a => a.AlertType);

            modelBuilder.Entity<InventoryLevelTable>()
                .HasKey(i => i.InventoryId);

            modelBuilder.Entity<InventoryAlertsTable>()
                .HasKey(a => a.AlertId);

            modelBuilder.Entity<ProductTable>()
                .HasKey(p => p.productId);

            // Auto-generate primary key
            modelBuilder.Entity<DashboardTable>()
                .Property(d => d.DashboardId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<InventoryLevelTable>()
                .Property(i => i.InventoryId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<InventoryAlertsTable>()
                .Property(a => a.AlertId)
                .ValueGeneratedOnAdd();

            // Map entities to new table names in the database
            modelBuilder.Entity<DashboardTable>()
                .ToTable("Dashboard");

            modelBuilder.Entity<DashboardTypeTable>()
                .ToTable("DashboardType");

            modelBuilder.Entity<AlertTypeTable>()
                .ToTable("InventoryAlertType");

            modelBuilder.Entity<InventoryLevelTable>()
                .ToTable("InventoryLevel");

            modelBuilder.Entity<InventoryAlertsTable>()
                .ToTable("InventoryAlerts");

            modelBuilder.Entity<ProductTable>().ToTable("Product");

            modelBuilder.Entity<ProductThresholdTable>().ToTable("ProductThreshold");
        }
    }
}