using NUnit.Framework;
using Currencies_API.Domain;
using Currencies_API.Models;
using System.Collections.Generic;

namespace UnitTests
{
    public class CurrencyManagerTests
    {
        CurrenciesManager currencyManager;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
        }

        [Test]
        public void CreateDictionaryFromValidRatesTest()
        {
            RatesBase rates = mockValidRates();
            currencyManager.CreateDictionaryFromRates(rates);

            Dictionary<string, Dictionary<string, decimal>> dictionary;
            dictionary = currencyManager.ExchangeRatesDictionary;

            Assert.IsTrue(dictionary.Count > 0);

            Assert.AreEqual(dictionary["USD"]["EUR"], 0.5f);
            Assert.AreEqual(dictionary["USD"]["CAD"], 0.75f);

            Assert.AreEqual(dictionary["EUR"]["USD"], 2f);

            Assert.AreEqual(dictionary["CAD"]["USD"], 1.33f);

            Assert.IsFalse(dictionary["EUR"].ContainsKey("CAD"));
            Assert.IsFalse(dictionary["CAD"].ContainsKey("EUR"));

            Assert.AreEqual(dictionary.Count, 3);

            Assert.AreEqual(dictionary["USD"].Count, 2);
            Assert.AreEqual(dictionary["EUR"].Count, 1);
            Assert.AreEqual(dictionary["CAD"].Count, 1);
        }

        [Test]
        public void CreateDictionaryFromInvalidRatesTest()
        {
            RatesBase rates = mockInvalidRates();
            currencyManager.CreateDictionaryFromRates(rates);

            Assert.Zero(currencyManager.ExchangeRatesDictionary.Count);
        }

        [Test]
        public void CreateDictionaryNullRates()
        {
            RatesBase rates = null;
            currencyManager.CreateDictionaryFromRates(rates);

            Assert.Zero(currencyManager.ExchangeRatesDictionary.Count);
        }

        [Test]
        public void CreateDictionaryEmptyRates()
        {
            RatesBase rates = new RatesBase();
            currencyManager.CreateDictionaryFromRates(rates);

            Assert.Zero(currencyManager.ExchangeRatesDictionary.Count);
        }

        private RatesBase mockValidRates()
        {
            RatesBase rates = new RatesBase();
            IndividualRate toAdd;
            rates.RateArray = new IndividualRate[4];

            toAdd = new IndividualRate();
            toAdd.from = "EUR";
            toAdd.to = "USD";
            toAdd.rate = 2m;
            rates.RateArray[0] = toAdd;

            toAdd = new IndividualRate();
            toAdd.from = "USD";
            toAdd.to = "EUR";
            toAdd.rate = 0.5m;
            rates.RateArray[1] = toAdd;

            toAdd = new IndividualRate();
            toAdd.from = "USD";
            toAdd.to = "CAD";
            toAdd.rate = 0.75m;
            rates.RateArray[2] = toAdd;

            toAdd = new IndividualRate();
            toAdd.from = "CAD";
            toAdd.to = "USD";
            toAdd.rate = 1.33m;
            rates.RateArray[3] = toAdd;

            return rates;
        }

        private RatesBase mockInvalidRates()
        {
            RatesBase rates = new RatesBase();
            IndividualRate toAdd;
            rates.RateArray = new IndividualRate[6];

            toAdd = new IndividualRate();
            toAdd.from = "USD";
            toAdd.to = "USD";
            toAdd.rate = 0.5m;
            rates.RateArray[0] = toAdd;

            toAdd = new IndividualRate();
            toAdd.from = "EUR";
            toAdd.to = "USD";
            toAdd.rate = -0.5m;
            rates.RateArray[1] = toAdd;

            toAdd = new IndividualRate();
            toAdd.from = null;
            toAdd.to = "USD";
            toAdd.rate = 0.75m;
            rates.RateArray[2] = toAdd;

            toAdd = new IndividualRate();
            toAdd.from = "USD";
            toAdd.to = null;
            toAdd.rate = 0.5m;
            rates.RateArray[3] = toAdd;

            toAdd = new IndividualRate();
            toAdd.from = string.Empty;
            toAdd.to = "EUR";
            toAdd.rate = 0.5m;
            rates.RateArray[4] = toAdd;

            toAdd = new IndividualRate();
            toAdd.from = "EUR";
            toAdd.to = string.Empty;
            toAdd.rate = 0.5m;
            rates.RateArray[5] = toAdd;

            return rates;
        }
    }
}