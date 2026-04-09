using FinanceManager.Domain.Entities;
using FinanceManager.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Bill> Bills { get; set; }
    public DbSet<BillOcorrence> BillOcorrences { get; set; }
    public DbSet<RecurrenceType> RecurrenceTypes { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
