using CleanBrilliantCompany.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace CleanBrilliantCompany.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<IngredientSDM> Ingredients { get; set; }

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
        }
    }
}