using ApiCaller;
using DataSource;
using Microsoft.AspNetCore.Mvc;
using PruebaTecnicaVueling.DataSource;
using PruebaTecnicaVueling.Domain;
using PruebaTecnicaVueling.Models;
//using System.Web.Http;

namespace PruebaTecnicaVueling.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CurrenciesController : ControllerBase
    {
        private readonly ILogger<CurrenciesController> _logger;
        private DataManagerBL dataManagerBL;
        private CurrenciesManager currenciesManager;
        public CurrenciesController(ILogger<CurrenciesController> logger)
        {
            _logger = logger;
            generateDataManager();
        }

        private void generateDataManager()
        {
            ApiXmlClient apiProcessor = new ApiXmlClient(DataManagerXml.baseAddress);
            XmlParser xmlParser = new XmlParser();

            DataManagerXml dataManagerXml = new DataManagerXml(apiProcessor, xmlParser);
            dataManagerBL = new DataManagerBL(dataManagerXml);

            CurrencyExchanger currencyExchanger = new CurrencyExchanger(2);
            currenciesManager = new CurrenciesManager(currencyExchanger);
        }

        [HttpGet(Name = "GetExchangeRates")]
        public IEnumerable<IndividualRate> GetRates()
        {
            RatesBase? result = dataManagerBL.GetRates().Result;

            if (result == null)
            {
                throw new System.Web.Http.HttpResponseException(System.Net.HttpStatusCode.InternalServerError);
            }

            return result.RateArray;
        }

        [HttpGet(Name = "GetTransactions")]
        public IEnumerable<IndividualTransaction> GetTransactions()
        {
            TransactionsBase? result = dataManagerBL.GetTransactions().Result;
            if (result == null)
            {
                throw new System.Web.Http.HttpResponseException(System.Net.HttpStatusCode.InternalServerError);
            }

            return result.transactionArray;
        }

        [HttpGet(Name = "GetTransactionsBySku")]
        public TransactionsSku GetTransactionsBySku(string sku)
        {
            const string CURRENCY_TO_CONVERT = "EUR";
            TransactionsSku result = new TransactionsSku();
            List<decimal> finalTransactions = new List<decimal>();

            IndividualTransaction[] transactionArray = dataManagerBL.GetTransactionsFileFirst().Result.transactionArray;
            IEnumerable<IndividualTransaction> filteredTransactions = transactionArray.Where(x => x.sku == sku).ToArray();

            currenciesManager.CreateDictionaryFromXMLRates(dataManagerBL.GetRates().Result);

            result.Sku = sku;
            result.Currency = CURRENCY_TO_CONVERT;
            result.TotalAmount = 0.0m;
            result.TotalErrors = 0;

            foreach (IndividualTransaction transaction in filteredTransactions)
            {
                decimal amountToAdd;
                if (transaction.currency != CURRENCY_TO_CONVERT)
                {
                    amountToAdd = currenciesManager.ExchangeCurrency(transaction.currency, CURRENCY_TO_CONVERT, transaction.amount);
                }
                else
                {
                    amountToAdd = transaction.amount;
                }

                if (amountToAdd > decimal.MinValue)
                {
                    result.TotalAmount += amountToAdd;
                    finalTransactions.Add(amountToAdd);
                }
                else
                {
                    result.TotalErrors++;
                }
            }

            result.TransactionAmounts = finalTransactions.ToArray();
            return result;
        }
    }
}
