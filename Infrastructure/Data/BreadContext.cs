using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class BreadContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<Input> Inputs { get; set; }
    public DbSet<InputIngredients> InputIngredients { get; set; }
    public DbSet<InputProducts> InputProducts { get; set; }
    public DbSet<Output> Outputs { get; set; }
    public DbSet<OutputIngredients> OutputIngredients { get; set; }
    public DbSet<OutputProducts> OutputProducts { get; set; }
    public DbSet<ProductIngredients> ProductIngredients { get; set; }
    public DbSet<User> Users { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
