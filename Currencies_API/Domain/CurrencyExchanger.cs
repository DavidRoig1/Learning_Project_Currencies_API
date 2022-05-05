namespace Currencies_API.Domain
{
    public class CurrencyExchanger : ICurrencyExchanger
    {
        public decimal ExchangeCurrency(decimal amount, decimal rate)
        {
            return (amount * rate);
        }

        public decimal RoundHalfToEven(decimal numberToRound, uint maxDecimals)
        {
            return Math.Round(numberToRound, (int)maxDecimals, MidpointRounding.ToEven);
        }
    }
}
