using Microsoft.EntityFrameworkCore;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options) { }

        public DbSet<ShippingAgent> ShippingAgents { get; set; } // References the ShippingAgent table

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShippingAgent>().ToTable("ShippingAgent"); // Maps to DB table
            base.OnModelCreating(modelBuilder);
        }
    }
}
