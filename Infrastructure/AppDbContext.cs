using Microsoft.EntityFrameworkCore;
using MiniMES_API.Entities;

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

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("product", "public");
            entity.HasKey(product => product.ProductId);

            entity.Property(product => product.ProductId)
                .ValueGeneratedOnAdd();
            entity.Property(product => product.ProductCode)
                .HasMaxLength(10)
                .IsRequired();
            entity.Property(product => product.ProductName)
                .HasMaxLength(200)
                .IsRequired();
            entity.Property(product => product.ProductWidth)
                .HasPrecision(10, 2);
            entity.Property(product => product.ProductHeight)
                .HasPrecision(10, 2);
            entity.Property(product => product.ProductWeight)
                .HasPrecision(10, 2);
            entity.Property(product => product.ProductUnitCost)
                .HasPrecision(15, 2);
            entity.Property(product => product.ProductImage)
                .HasColumnType("bytea");
            entity.Property(product => product.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .IsRequired();
            entity.Property(product => product.IsActive)
                .HasDefaultValue(true)
                .IsRequired();

            entity.HasIndex(product => product.ProductCode)
                .IsUnique();

            entity.HasOne<ProductType>()
                .WithMany()
                .HasForeignKey(product => product.ProductTypeId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<ProductType>(entity =>
        {
            entity.ToTable("product_type", "public");
            entity.HasKey(productType => productType.ProductTypeId);

            entity.Property(productType => productType.ProductTypeId)
                .ValueGeneratedOnAdd();
            entity.Property(productType => productType.ProductTypeCode)
                .HasMaxLength(20)
                .IsRequired();
            entity.Property(productType => productType.ProductTypeName)
                .HasMaxLength(200)
                .IsRequired();
            entity.Property(productType => productType.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .IsRequired();
            entity.Property(productType => productType.IsActive)
                .HasDefaultValue(true)
                .IsRequired();

            entity.HasIndex(productType => productType.ProductTypeCode)
                .IsUnique();
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.ToTable("company", "public");
            entity.HasKey(company => company.CompanyId);

            entity.Property(company => company.CompanyId)
                .ValueGeneratedOnAdd();
            entity.Property(company => company.CompanyCode)
                .HasMaxLength(20)
                .IsRequired();
            entity.Property(company => company.CompanyName)
                .HasMaxLength(200)
                .IsRequired();
            entity.Property(company => company.BusinessNumber)
                .HasMaxLength(20);
            entity.Property(company => company.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .IsRequired();
            entity.Property(company => company.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .IsRequired();
            entity.Property(company => company.IsActive)
                .HasDefaultValue(true)
                .IsRequired();

            entity.HasIndex(company => company.CompanyCode)
                .IsUnique();
        });

        modelBuilder.Entity<CompanyUser>(entity =>
        {
            entity.ToTable("company_user", "public");
            entity.HasKey(companyUser => companyUser.CompanyUserId);

            entity.Property(companyUser => companyUser.CompanyUserId)
                .ValueGeneratedOnAdd();
            entity.Property(companyUser => companyUser.LoginId)
                .HasMaxLength(100)
                .IsRequired();
            entity.Property(companyUser => companyUser.PasswordHash)
                .HasMaxLength(500)
                .IsRequired();
            entity.Property(companyUser => companyUser.UserName)
                .HasMaxLength(100)
                .IsRequired();
            entity.Property(companyUser => companyUser.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .IsRequired();
            entity.Property(companyUser => companyUser.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .IsRequired();
            entity.Property(companyUser => companyUser.IsActive)
                .HasDefaultValue(true)
                .IsRequired();

            entity.HasIndex(companyUser => companyUser.LoginId)
                .IsUnique();

            entity.HasOne(companyUser => companyUser.Company)
                .WithMany()
                .HasForeignKey(companyUser => companyUser.CompanyId)
                .OnDelete(DeleteBehavior.NoAction);
        });
    }
}
