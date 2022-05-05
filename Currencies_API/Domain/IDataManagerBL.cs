using Currencies_API.Models;

namespace Currencies_API.Domain
{
    public interface IDataManagerBL
    {
        Task<RatesBase?> GetRates();
        Task<TransactionsBase?> GetTransactions();
        Task<TransactionsBase?> GetTransactionsFileFirst();
    }
}