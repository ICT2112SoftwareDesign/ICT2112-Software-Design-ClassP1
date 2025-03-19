using Microsoft.EntityFrameworkCore;
using CleanBrilliantCompany.Models;
using System.Collections.Generic;

namespace CleanBrilliantCompany.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<DashboardTable> DashboardTable { get; set; }
        public DbSet<DashboardTableType> DashboardTableType { get; set; }
        public DbSet<StockStatus> StockStatus { get; set; }
        public DbSet<AlertType> AlertType { get; set; }
        public DbSet<InventoryLevel> InventoryLevel { get; set; }
        public DbSet<InventoryAlerts> InventoryAlerts { get; set; }

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define DashboardId as the primary key
            modelBuilder.Entity<InventoryDashboardRDM>()
                .HasKey(d => d.DashboardId);

            modelBuilder.Entity<DashboardTable>()
                .Property(d => d.DashboardId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<InventoryLevel>()
                .Property(i => i.InventoryId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<InventoryAlerts>()
                .Property(a => a.AlertId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<StockStatus>()
                .HasKey(s => s.StockCode);

            modelBuilder.Entity<AlertType>()
                .HasKey(a => a.TypeCode);

            // Relationships between tables

            //modelBuilder.Entity<InventoryLevel>()
            //    .HasOne(i => i.StockStatus)
            //    .WithMany()
            //    .HasForeignKey(i => i.StockCode);

            //modelBuilder.Entity<InventoryLevel>()
            //    .HasOne(i => i.DashboardTable)
            //    .WithMany(d => d.InventoryLevels)
            //    .HasForeignKey(i => i.DashboardId);

            //modelBuilder.Entity<InventoryAlerts>()
            //    .HasOne(a => a.InventoryLevel)
            //    .WithMany(i => i.AlertsTypes)
            //    .HasForeignKey(a => a.InventoryId);

            //modelBuilder.Entity<InventoryAlerts>()
            //    .HasOne(a => a.AlertType)
            //    .WithMany()
            //    .HasForeignKey(a => a.AlertTypeCode);

            // VERIFY AGAIN
            //modelBuilder.Entity<DashboardTable>()
            //    .HasOne(d => d.Type)
            //    .WithMany()
            //    .HasForeignKey(d => d.TypeId);

            modelBuilder.Entity<InventoryLevel>()
                .HasOne(i => i.StockStatus)
                .WithMany()
                .HasForeignKey(i => i.StockCode);

            //modelBuilder.Entity<InventoryLevel>()
            //    .HasOne(i => i.DashboardTable)
            //    .WithMany(d => d.InventoryLevels)
            //    .HasForeignKey(i => i.DashboardId);

            //modelBuilder.Entity<InventoryAlerts>()
            //    .HasOne(a => a.InventoryLevel)
            //    .WithMany(i => i.AlertType)
            //    .HasForeignKey(a => a.InventoryId);

            modelBuilder.Entity<InventoryAlerts>()
                .HasOne(a => a.AlertType)
                .WithMany()
                .HasForeignKey(a => a.AlertTypeCode);
        }
    }
}