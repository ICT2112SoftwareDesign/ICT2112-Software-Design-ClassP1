using CleanBrilliantCompany.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace CleanBrilliantCompany.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<IngredientSDM> Ingredients { get; set; }
        public DbSet<GoalsSDM> Goals { get; set; }
        
        // We need this just for mapping - this doesn't create a new entity since other team handles it
        // This is just for the ORM to access the existing Product table
        public DbSet<ProductMapping> Products { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the Ingredient entity
            modelBuilder.Entity<IngredientSDM>()
                .HasKey(i => i.IngredientId);

            modelBuilder.Entity<IngredientSDM>()
                .Property(i => i.IngredientName)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<IngredientSDM>()
                .Property(i => i.IngredientToxicity)
                .IsRequired();
                
            // Configure the Product mapping
            modelBuilder.Entity<ProductMapping>()
                .HasKey(p => p.ProductId);
                
            modelBuilder.Entity<ProductMapping>()
                .ToTable("Product"); // Map to the existing Product table
        }
    }
    
    // Basic entity for mapping to the product table only - does not add functionality
    [System.ComponentModel.DataAnnotations.Schema.Table("Product")]
    public class ProductMapping
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
    }
}