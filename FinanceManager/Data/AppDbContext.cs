using FinanceManager.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<Bill> Bills { get; set; }
    public DbSet<BillOcorrence> BillOcorrences { get; set; }
    public DbSet<RecurrenceType> RecurrenceTypes { get; set; }
    public DbSet<User> Users { get; set; }

    private readonly RecurrenceType[] _recurrenceTypes = new[]
    {
        new RecurrenceType { Id = 1, Type = "Diário", DaysInterval = 1 },
        new RecurrenceType { Id = 2, Type = "Semanal", DaysInterval = 7 },
        new RecurrenceType { Id = 3, Type = "Quinzenal", DaysInterval = 14 },
        new RecurrenceType { Id = 4, Type = "Mensal", DaysInterval = 30 },
        new RecurrenceType { Id = 5, Type = "Trimestral", DaysInterval = 90 },
        new RecurrenceType { Id = 6, Type = "Semestral", DaysInterval = 182 },
        new RecurrenceType { Id = 7, Type = "Anual", DaysInterval = 365 },
    };
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RecurrenceType>().HasData(_recurrenceTypes);
        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
    }
}