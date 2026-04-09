using FinanceManager.Domain.Entities;

namespace FinanceManager.Domain.Factories;

public interface IBillOcorrenceFactory
{
    IEnumerable<BillOcorrence> Generate(Bill bill);
}
