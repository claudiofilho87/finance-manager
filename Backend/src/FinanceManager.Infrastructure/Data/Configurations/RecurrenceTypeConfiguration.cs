using FinanceManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceManager.Infrastructure.Data.Configurations;

public class RecurrenceTypeConfiguration : IEntityTypeConfiguration<RecurrenceType>
{
    public void Configure(EntityTypeBuilder<RecurrenceType> builder)
    {
        builder.ToTable("recurrence_types");

        builder.HasKey(rt => rt.Id);
        builder.Property(rt => rt.Id).HasColumnName("id");
        builder.Property(rt => rt.Type).HasColumnName("type").HasMaxLength(255);
        builder.Property(rt => rt.DaysInterval).HasColumnName("days_interval");

        builder.HasData(
            new RecurrenceType { Id = 1, Type = "Diário", DaysInterval = 1 },
            new RecurrenceType { Id = 2, Type = "Semanal", DaysInterval = 7 },
            new RecurrenceType { Id = 3, Type = "Quinzenal", DaysInterval = 14 },
            new RecurrenceType { Id = 4, Type = "Mensal", DaysInterval = 30 },
            new RecurrenceType { Id = 5, Type = "Trimestral", DaysInterval = 90 },
            new RecurrenceType { Id = 6, Type = "Semestral", DaysInterval = 182 },
            new RecurrenceType { Id = 7, Type = "Anual", DaysInterval = 365 }
        );
    }
}
