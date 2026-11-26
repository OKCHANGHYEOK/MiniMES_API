using Microsoft.EntityFrameworkCore;
using MiniMES_API.Entities;
using MiniMES_API.ModelBuilders;

namespace MiniMES_API.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductType> ProductTypes => Set<ProductType>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<CompanyUser> CompanyUsers => Set<CompanyUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ProductModelBuilder.SetModelBuilder(modelBuilder);
        ProductTypeModelBuilder.SetModelBuilder(modelBuilder);
        CompanyModelBuilder.SetModelBuilder(modelBuilder);
        CompanyUserModelBuilder.SetModelBuilder(modelBuilder);
    }
}
