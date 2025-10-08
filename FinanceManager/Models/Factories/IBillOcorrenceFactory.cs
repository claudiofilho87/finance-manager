namespace FinanceManager.Models.Factories;

public interface IBillOcorrenceFactory
{
    public IEnumerable<BillOcorrence> Generate(Bill bill);
}