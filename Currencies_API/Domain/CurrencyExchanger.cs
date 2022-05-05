namespace PruebaTecnicaVueling.Domain
{
    public class CurrencyExchanger
    {
        private uint maxDecimals;
        public CurrencyExchanger(uint maxDecimals)
        {
            this.maxDecimals = maxDecimals;
        }

        public decimal ExchangeCurrency(decimal amount, decimal rate)
        {
            return (amount * rate);
        }

        public decimal RoundHalfToEven(decimal numberToRound)
        {
            return Math.Round(numberToRound, (int)maxDecimals, MidpointRounding.ToEven);
        }
    }
}
