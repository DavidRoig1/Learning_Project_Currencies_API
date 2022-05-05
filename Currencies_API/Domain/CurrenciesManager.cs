using PruebaTecnicaVueling.Models;

namespace PruebaTecnicaVueling.Domain
{
    public class CurrenciesManager
    {
        private readonly ILogger<CurrencyExchanger> logger;

        private readonly CurrencyExchanger currencyExchanger;

        private readonly (string currency, decimal amount) NO_TRANSACTION = (string.Empty, decimal.MinValue);

        /// <summary>
        /// I used a nested dictionary for this variable because it seems like the best balance between
        /// code readibility and performance. If performance needed to be improved, I would probably
        /// use a collection of ValueTuples instead. 
        /// 
        /// Although, in a real life scenario this would probably be done by querying the database.
        /// I don't think it's possible to deploy a database in this Technical Evaluation.
        /// </summary>
        public Dictionary<string, Dictionary<string, decimal>> ExchangeRatesDictionary;


        public CurrenciesManager(CurrencyExchanger currencyExchanger)
        {
            this.currencyExchanger = currencyExchanger;
        }

        public decimal ExchangeCurrency(string currencyFrom, string currencyTo, decimal amount)
        {
            decimal result;

            //The best case scenario in which an exchange is inmediately found is used on a separate if
            //in order to make use of the O(n) search time that the dictionary offers
            if (ExchangeRatesDictionary[currencyFrom].ContainsKey(currencyTo) == true)
            {
                result = currencyExchanger.ExchangeCurrency(amount, ExchangeRatesDictionary[currencyFrom][currencyTo]);
            }
            else
            {
                result = ExchangeCurrenciesIndirectly(currencyFrom, currencyTo, amount);
            }

            return currencyExchanger.RoundHalfToEven(result);
        }

        // Cambiar metodo a generar path de conversiones
        public decimal ExchangeCurrenciesIndirectly(string currencyFrom, string currencyTo, decimal amount)
        {
            decimal result = decimal.MinValue;

            Queue<string> currentCurrenciesPath = new Queue<string>();
            Stack<string> currenciesStack = new Stack<string>();
            HashSet<string> checkedCurrencies = new HashSet<string>();

            //Add initial children to stack
            foreach (var item in ExchangeRatesDictionary[currencyFrom])
            {
                currenciesStack.Push(item.Key);
            }
            checkedCurrencies.Add(currencyFrom);
            
            // Calculate conversion path
            while (currenciesStack.Count > 0)
            {
                if (checkedCurrencies.Contains(currenciesStack.Peek()) == false)
                {
                    int addedKeys = 0;
                    string item = currenciesStack.Pop();
                    checkedCurrencies.Add(item);
                    currentCurrenciesPath.Enqueue(item);

                    var currentSons = ExchangeRatesDictionary[item];
                    if (currentSons.ContainsKey(currencyTo))
                    {
                        break;
                    }

                    if(currentSons.Count > 0)
                    {
                        foreach (var grandchildren in currentSons)
                        {
                            if (checkedCurrencies.Contains(grandchildren.Key) == false)
                            {
                                addedKeys++;
                                currenciesStack.Push(grandchildren.Key);
                            }
                        }
                    }
                    if(addedKeys == 0)
                    {
                        currentCurrenciesPath.Clear();
                    }
                }
                else
                {
                    currenciesStack.Pop();
                }
            }

            if(currentCurrenciesPath.Count > 0)
            {
                currentCurrenciesPath.Enqueue(currencyTo);
                List<string> list = currentCurrenciesPath.ToList();
                list.Insert(0, currencyFrom);
                result = amount;
                
                for (int i = 1; i < list.Count; i++)
                {
                    decimal rateToExchange = ExchangeRatesDictionary[list[i - 1]][list[i]];
                    result = currencyExchanger.ExchangeCurrency(result, rateToExchange);
                }
            }

            return result;
        }

        public Dictionary<string, Dictionary<string, decimal>> CreateDictionaryFromXMLRates(RatesBase rates)
        {
            ExchangeRatesDictionary = new Dictionary<string, Dictionary<string, decimal>>();

            try
            {
                foreach (IndividualRate singleRate in rates.RateArray)
                {
                    checkAndAddSingleRateToDictionary(singleRate);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error while accessing the rates array");
                logger.LogError(ex.ToString());
            }

            return ExchangeRatesDictionary;
        }

        private void checkAndAddSingleRateToDictionary(IndividualRate singleRate)
        {
            try
            {
                string fromId = singleRate.from;
                if (singleRateIsValid(singleRate))
                {
                    createSecondDictionaryIfNotExists(fromId);

                    ExchangeRatesDictionary[fromId].Add(singleRate.to, singleRate.rate);
                }
                else
                {
                    logger.LogWarning($"Invalid Exchange rate: from{singleRate.from}, to: {singleRate.to},rate {singleRate.rate}");
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error while processing a rate", singleRate);
                logger.LogError(ex.ToString());
            }
        }

        private bool singleRateIsValid(IndividualRate rate)
        {
            bool isValid = false;

            if (string.IsNullOrEmpty(rate.from) == false && string.IsNullOrEmpty(rate.to) == false)
            {
                if (string.Equals(rate.from, rate.to) == false)
                    if (rate.rate > 0)
                        isValid = true;
            }

            return isValid;
        }

        private void createSecondDictionaryIfNotExists(string fromId)
        {
            if (ExchangeRatesDictionary.ContainsKey(fromId) == false)
            {
                ExchangeRatesDictionary.Add(fromId, new Dictionary<string, decimal>());
            }
        }
    }
}
