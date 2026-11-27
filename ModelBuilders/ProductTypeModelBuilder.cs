using Microsoft.EntityFrameworkCore;
using MiniMES_API.Entities;

namespace MiniMES_API.ModelBuilders;

public class ProductTypeModelBuilder : IEntityModelBuilder
{
    public static void SetModelBuilder(ModelBuilder modelBuilder)
    {
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
    }
}
