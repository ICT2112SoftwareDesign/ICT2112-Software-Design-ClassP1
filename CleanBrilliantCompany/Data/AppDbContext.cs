using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using CleanBrilliantCompany.Entities;

namespace CleanBrilliantCompany.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<DashboardTable> DashboardTable { get; set; }
        public DbSet<DashboardTypeTable> DashboardTableType { get; set; }
        public DbSet<StockStatusTable> StockStatusTable { get; set; }
        public DbSet<AlertTypeTable> AlertTypeTable { get; set; }
        public DbSet<InventoryLevelTable> InventoryLevelTable { get; set; }
        public DbSet<InventoryAlertsTable> InventoryAlertsTable { get; set; }

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define primary key
            modelBuilder.Entity<DashboardTable>()
                .HasKey(d => d.DashboardId);

            modelBuilder.Entity<DashboardTypeTable>()
                .HasKey(t => t.TypeId);

            modelBuilder.Entity<StockStatusTable>()
                .HasKey(s => s.StockCode);
            
            modelBuilder.Entity<AlertTypeTable>()
                .HasKey(a => a.AlertType);
            
            modelBuilder.Entity<InventoryLevelTable>()
                .HasKey(i => i.InventoryId);
            
            modelBuilder.Entity<InventoryAlertsTable>()
                .HasKey(a => a.AlertId);

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

            modelBuilder.Entity<StockStatusTable>()
                .ToTable("StockStatus");

            modelBuilder.Entity<AlertTypeTable>()
                .ToTable("InventoryAlertType");

            modelBuilder.Entity<InventoryLevelTable>()
                .ToTable("InventoryLevel");

            modelBuilder.Entity<InventoryAlertsTable>()
                .ToTable("InventoryAlerts");

            // Relationships between tables
            // One-to-many relationship between DashboardTypeTable and DashboardTable
            // One DashboardTypeTable can have many DashboardTable
            modelBuilder.Entity<DashboardTable>()
                .HasOne(d => d.Type)
                .WithMany()
                .HasForeignKey(d => d.TypeId);

            // One-to-many relationship between InventoryLevelTable and StockStatusTable
            // One StockStatusTable can have many InventoryLevelTable
            modelBuilder.Entity<InventoryLevelTable>()
                .HasOne(i => i.StockStatus)
                .WithMany()
                .HasForeignKey(i => i.StockCode);

            // One-to-many relationship between InventoryLevelTable and DashboardTable
            // One DashboardTable can have many InventoryLevels
            modelBuilder.Entity<InventoryLevelTable>()
                .HasOne(i => i.DashboardTable)
                .WithMany(d => d.InventoryLevels)
                .HasForeignKey(i => i.DashboardId);

            // One-to-many relationship between InventoryAlertsTable and InventoryLevelTable
            // One InventoryLevelTable can have many InventoryAlerts
            modelBuilder.Entity<InventoryAlertsTable>()
                .HasOne(a => a.InventoryLevel)
                .WithMany(i => i.AlertTypes)
                .HasForeignKey(a => a.InventoryId);

            // One-to-many relationship between InventoryAlertsTable and AlertTypeTable
            // One AlertTypeTable can have many InventoryAlertsTable
            modelBuilder.Entity<InventoryAlertsTable>()
                .HasOne(a => a.AlertTypes)
                .WithMany()
                .HasForeignKey(a => a.AlertType);
        }
    }
}