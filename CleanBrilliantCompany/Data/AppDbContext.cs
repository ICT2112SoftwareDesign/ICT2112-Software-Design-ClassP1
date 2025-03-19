using Microsoft.EntityFrameworkCore;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Data
{
    public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<ReorderRequest> ReorderRequests { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReorderRequest>().ToTable("ReorderRequest");
        
        base.OnModelCreating(modelBuilder);
    }
}
}
