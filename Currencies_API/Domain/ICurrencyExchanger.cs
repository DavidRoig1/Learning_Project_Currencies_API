namespace Currencies_API.Domain
{
    public interface ICurrencyExchanger
    {
        decimal ExchangeCurrency(decimal amount, decimal rate);
        decimal RoundHalfToEven(decimal numberToRound, uint maxDecimals);
    }
}