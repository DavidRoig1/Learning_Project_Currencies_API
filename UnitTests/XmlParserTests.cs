using DataSource;
using NUnit.Framework;
using PruebaTecnicaVueling.Models;
using System;

namespace UnitTests
{
    public class XmlParserTests
    {
        XmlParser xmlParser;

        [OneTimeSetUp]
        public void InitialSetup()
        {
            xmlParser = new XmlParser();
        }

        [Test]
        public void ValidRatesTest()
        {
            string validRatesPath = "files/rates.xml";
            RatesBase? rates = xmlParser.SerializeObjectFromFile<RatesBase>(validRatesPath);

            Assert.NotNull(rates);
            Assert.IsTrue(rates.RateArray.Length > 0);
        }

        [Test]
        public void WrongFormatRatesTest()
        {
            string wrongFromatRatesPath = "files/WrongRates.xml";
            RatesBase? rates = xmlParser.SerializeObjectFromFile<RatesBase>(wrongFromatRatesPath);

            Assert.IsNull(rates);
        }

        [Test]
        public void ValidTransactionsTest()
        {
            string validRatesPath = "files/transactions.xml";
            TransactionsBase? transactionsXML = xmlParser.SerializeObjectFromFile<TransactionsBase>(validRatesPath);

            Assert.NotNull(transactionsXML);
            Assert.IsTrue(transactionsXML.transactionArray.Length > 0);
        }

        [Test]
        public void WrongFormatTransactionsTest()
        {
            string wrongFromatRatesPath = "files/WrongTransactions.xml";
            TransactionsBase? transactions = xmlParser.SerializeObjectFromFile<TransactionsBase>(wrongFromatRatesPath);

            Assert.IsNull(transactions);
        }

        [Test]
        public void InvalidDocumentTest()
        {
            string wrongFromatRatesPath = "files/InvalidDocument.xml";

            Assert.Throws<InvalidOperationException>(() => xmlParser.SerializeObjectFromFile<RatesBase>(wrongFromatRatesPath));
        }

        [Test]
        public void NullPathTest()
        {
            // The object returned  in this case is irrelevant as long as it's nullable
            Assert.Throws<ArgumentNullException>(() => xmlParser.SerializeObjectFromFile<RatesBase>(null));
        }

        [Test]
        public void NonExistantPathTest()
        {
            // The object returned in this case is irrelevant
            Assert.Throws<System.IO.FileNotFoundException>(() => xmlParser.SerializeObjectFromFile<RatesBase>("asjkodpjasopdj;;;"));
        }
    }
}