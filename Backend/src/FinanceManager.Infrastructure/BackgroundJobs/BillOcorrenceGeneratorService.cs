using FinanceManager.Domain.Entities;
using FinanceManager.Domain.Enums;
using FinanceManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FinanceManager.Infrastructure.BackgroundJobs;

public class BillOcorrenceGeneratorService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public BillOcorrenceGeneratorService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var timer = new PeriodicTimer(TimeSpan.FromDays(1));
        do
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await GenerateOccurrencesAsync(context);
        } while (await timer.WaitForNextTickAsync(stoppingToken));
    }

    private async Task GenerateOccurrencesAsync(AppDbContext context)
    {
        var limitDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(6));
        var bills = await context.Bills
            .Include(b => b.BillOcorrences)
            .Include(b => b.RecurrenceType)
            .Where(b => b.RepeatCount == null)
            .Where(b => !b.BillOcorrences.Any() ||
                        b.BillOcorrences.Max(o => o.Date) <= limitDate)
            .ToListAsync();

        var now = DateTime.UtcNow;
        var ocorrences = new List<BillOcorrence>();

        foreach (var bill in bills)
        {
            var lastDate = bill.BillOcorrences
                .OrderByDescending(o => o.Date)
                .FirstOrDefault()?.Date ?? bill.StartDate;

            for (int i = 1; i <= 6; i++)
            {
                var newDate = bill.RecurrenceType!.DaysInterval switch
                {
                    (int)RecurrenceTypeEnum.Monthly => lastDate.AddMonths(i),
                    (int)RecurrenceTypeEnum.Quarterly => lastDate.AddMonths(i * 3),
                    (int)RecurrenceTypeEnum.Semiannual => lastDate.AddMonths(i * 6),
                    (int)RecurrenceTypeEnum.Yearly => lastDate.AddYears(i),
                    _ => lastDate.AddDays(i * bill.RecurrenceType.DaysInterval)
                };

                ocorrences.Add(new BillOcorrence
                {
                    BillId = bill.Id,
                    Date = newDate,
                    Status = BillStatus.Pending,
                    ValueCents = bill.ValueCents,
                    CreatedAt = now,
                    UpdatedAt = now
                });
            }
        }

        if (ocorrences.Any())
        {
            await context.BillOcorrences.AddRangeAsync(ocorrences);
            await context.SaveChangesAsync();
        }
    }
}
