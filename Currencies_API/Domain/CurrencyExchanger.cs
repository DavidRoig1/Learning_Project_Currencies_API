namespace Currencies_API.Domain
{
    public class CurrencyExchanger : ICurrencyExchanger
    {
        public const uint DEFAULT_MAX_DECIMALS = 2;
        public decimal ExchangeCurrency(decimal amount, decimal rate)
        {
            return (amount * rate);
        }

        public decimal RoundHalfToEven(decimal numberToRound, uint maxDecimals = DEFAULT_MAX_DECIMALS)
        {
            return Math.Round(numberToRound, (int)maxDecimals, MidpointRounding.ToEven);
        }
    }
}
