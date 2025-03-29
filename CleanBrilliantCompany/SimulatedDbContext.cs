using Microsoft.EntityFrameworkCore;

public class SimulatedDbContext : DbContext
{
    public DbSet<BatchTable> Batches { get; set; } 
    public DbSet<StockHistoryTable> StockHistories { get; set; } 
    public DbSet<ProductTable> Products { get; set; } 
    public SimulatedDbContext(DbContextOptions<SimulatedDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    
        modelBuilder.Entity<BatchTable>().ToTable("ProductBatch"); 
        modelBuilder.Entity<StockHistoryTable>().ToTable("StockHistory");
        modelBuilder.Entity<ProductTable>().ToTable("Product"); 

        modelBuilder.Entity<BatchTable>()
            .HasKey(b => b.batchCode);  
        
        modelBuilder.Entity<StockHistoryTable>() 
            .HasKey(s => s.stockId); 
        
        modelBuilder.Entity<ProductTable>() 
            .HasKey(p => p.productId); 

        
        base.OnModelCreating(modelBuilder);
    }
}
