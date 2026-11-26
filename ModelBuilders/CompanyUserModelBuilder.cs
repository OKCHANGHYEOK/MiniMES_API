using Microsoft.EntityFrameworkCore;
using MiniMES_API.Entities;

namespace MiniMES_API.ModelBuilders;

public class CompanyUserModelBuilder : IEntityModelBuilder
{
    public static void SetModelBuilder(ModelBuilder modelBuilder)
    {
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
            entity.Ignore(companyUser => companyUser.PasswordOrigin);
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
