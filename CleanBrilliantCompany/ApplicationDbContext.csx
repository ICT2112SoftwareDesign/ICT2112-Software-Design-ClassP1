using Microsoft.EntityFrameworkCore;
public class ApplicationDbContext : DbContext
{
    public DbSet<DashboardTable> Dashboards { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DashboardTable>().ToTable("Dashboard");

        modelBuilder.Entity<DashboardTable>()
            .HasKey(d => d.DashboardId);

        base.OnModelCreating(modelBuilder);
    }
}