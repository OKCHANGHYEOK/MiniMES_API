using Microsoft.EntityFrameworkCore;
using MiniMES_API.Entities;

namespace MiniMES_API.ModelBuilders;

public class CompanyModelBuilder : IEntityModelBuilder
{
    public static void SetModelBuilder(ModelBuilder modelBuilder)
    {
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
    }
}
