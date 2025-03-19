using Microsoft.EntityFrameworkCore;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<InventoryDashboardRDM> InventoryDashboards { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InventoryDashboardRDM>()
                .Property(d => d.DashboardId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<InventoryDashboardRDM>()
                .Ignore(d => d.StockLevel);
            modelBuilder.Entity<InventoryDashboardRDM>()
                .Ignore(d => d.Threshold);
            modelBuilder.Entity<InventoryDashboardRDM>()
                .Ignore(d => d.ReplenishmentStatus);
        }
    }
}