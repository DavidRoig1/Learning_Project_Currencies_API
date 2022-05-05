using Currencies_API.Models;

namespace Currencies_API.Domain
{
    public interface ICurrenciesManager
    {
        Dictionary<string, Dictionary<string, decimal>> CreateDictionaryFromRates(RatesBase rates);
        decimal ExchangeCurrenciesIndirectly(string currencyFrom, string currencyTo, decimal amount);
        decimal ExchangeCurrency(string currencyFrom, string currencyTo, decimal amount);
    }
}