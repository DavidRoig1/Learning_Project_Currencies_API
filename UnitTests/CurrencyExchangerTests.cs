using NUnit.Framework;
using PruebaTecnicaVueling.Domain;

namespace UnitTests
{
    public class CurrencyExchangerTests
    {
        CurrencyExchanger currencyExchanger;
        uint maxDecimals = 2;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            currencyExchanger = new CurrencyExchanger(maxDecimals);
        }

        [Test]
        public void RoundHalfToEvenTest()
        {
            decimal result;

            result = currencyExchanger.RoundHalfToEven(10.005m);
            Assert.AreEqual(10.0m, result);

            result = currencyExchanger.RoundHalfToEven(11.355m);
            Assert.AreEqual(11.36m, result);

            result = currencyExchanger.RoundHalfToEven(3.505m);
            Assert.AreEqual(3.50m, result);

            result = currencyExchanger.RoundHalfToEven(3.4955555m);
            Assert.AreEqual(3.50m, result);
        }
    }
}