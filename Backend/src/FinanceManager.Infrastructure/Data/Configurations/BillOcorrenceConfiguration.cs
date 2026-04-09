using FinanceManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceManager.Infrastructure.Data.Configurations;

public class BillOcorrenceConfiguration : IEntityTypeConfiguration<BillOcorrence>
{
    public void Configure(EntityTypeBuilder<BillOcorrence> builder)
    {
        builder.ToTable("bill_ocorrences");

        builder.HasKey(bo => bo.Id);
        builder.Property(bo => bo.Id).HasColumnName("id");
        builder.Property(bo => bo.BillId).HasColumnName("bill_id");
        builder.Property(bo => bo.Date).HasColumnName("date");
        builder.Property(bo => bo.Status).HasColumnName("status");
        builder.Property(bo => bo.ValueCents).HasColumnName("value_cents");
        builder.Property(bo => bo.Observation).HasColumnName("observation");
        builder.Property(bo => bo.CreatedAt).HasColumnName("created_at");
        builder.Property(bo => bo.UpdatedAt).HasColumnName("updated_at");

        builder.HasOne(bo => bo.Bill)
            .WithMany(b => b.BillOcorrences)
            .HasForeignKey(bo => bo.BillId);
    }
}
