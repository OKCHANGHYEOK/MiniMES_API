using Microsoft.EntityFrameworkCore;
using MiniMES_API.Entities;

namespace MiniMES_API.ModelBuilders;

public class ProductModelBuilder : IEntityModelBuilder
{
    public static void SetModelBuilder(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("product", "public");
            entity.HasKey(product => product.ProductId);

            entity.Property(product => product.ProductId).ValueGeneratedOnAdd();
            entity.Property(product => product.ProductCode)
                  .HasMaxLength(10)
                  .IsRequired();
           
            entity.Property(product => product.ProductName)
                  .HasMaxLength(200)
                  .IsRequired();

            entity.Property(product => product.ProductWidth).HasPrecision(10, 2);
            entity.Property(product => product.ProductHeight).HasPrecision(10, 2);
            entity.Property(product => product.ProductWeight).HasPrecision(10, 2);
            entity.Property(product => product.ProductUnitCost).HasPrecision(15, 2);
            entity.Property(product => product.ProductImage).HasColumnType("bytea");
            entity.Property(product => product.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP").IsRequired();
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
    }
}
