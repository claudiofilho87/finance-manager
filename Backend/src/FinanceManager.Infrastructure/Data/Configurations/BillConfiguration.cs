using FinanceManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceManager.Infrastructure.Data.Configurations;

public class BillConfiguration : IEntityTypeConfiguration<Bill>
{
    public void Configure(EntityTypeBuilder<Bill> builder)
    {
        builder.ToTable("bills");

        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id).HasColumnName("id");
        builder.Property(b => b.Name).HasColumnName("name").HasMaxLength(255);
        builder.Property(b => b.Description).HasColumnName("description");
        builder.Property(b => b.ValueCents).HasColumnName("value_cents");
        builder.Property(b => b.StartDate).HasColumnName("start_date");
        builder.Property(b => b.RecurrenceTypeId).HasColumnName("recurrence_type_id");
        builder.Property(b => b.RepeatCount).HasColumnName("repeat_count");
        builder.Property(b => b.UserId).HasColumnName("user_id");
        builder.Property(b => b.IsActive).HasColumnName("is_active");
        builder.Property(b => b.CreatedAt).HasColumnName("created_at");
        builder.Property(b => b.UpdatedAt).HasColumnName("updated_at");

        builder.HasOne(b => b.RecurrenceType)
            .WithMany(rt => rt.Bills)
            .HasForeignKey(b => b.RecurrenceTypeId);

        builder.HasOne(b => b.User)
            .WithMany(u => u.Bills)
            .HasForeignKey(b => b.UserId);
    }
}
