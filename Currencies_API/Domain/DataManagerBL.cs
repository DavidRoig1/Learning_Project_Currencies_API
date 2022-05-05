using Currencies_API.DataSource;
using Currencies_API.Models;

namespace Currencies_API.Domain
{
    public class DataManagerBL : IDataManagerBL
    {
        IDataManager dataManager;

        private const string RATES_URI = "rates";
        private const string TRANSACTIONS_URI = "transactions";
        public DataManagerBL(IDataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        public async Task<RatesBase?> GetRates()
        {
            RatesBase? result;

            result = await GetDataApiFirst<RatesBase>(RATES_URI);

            return result;
        }

        /// <summary>
        /// The parameter from file is added to 
        /// </summary>
        /// <param name="fromFile"></param>
        /// <returns></returns>
        public async Task<TransactionsBase?> GetTransactions()
        {
            TransactionsBase? result;

            result = await GetDataApiFirst<TransactionsBase>(TRANSACTIONS_URI);

            return result;
        }

        /// <summary>
        /// This method is created in order to more easily filter
        /// </summary>
        /// <returns></returns>
        public async Task<TransactionsBase?> GetTransactionsFileFirst()
        {
            TransactionsBase? result;

            result = await GetDataFileFirst<TransactionsBase>(TRANSACTIONS_URI);

            return result;
        }

        private async Task<T?> GetDataApiFirst<T>(string uri)
        {
            T? data = default(T?);

            data = await GetDataFromApi<T>(uri);

            if (data == null)
            {
                data = GetDataFromFile<T>(uri);
            }

            return data;
        }

        private async Task<T?> GetDataFileFirst<T>(string uri)
        {
            T? data = default(T?);

            data = GetDataFromFile<T>(uri);

            if (data == null)
            {
                data = await GetDataFromApi<T>(uri);
            }

            return data;
        }

        private async Task<T?> GetDataFromApi<T>(string uri)
        {
            T? data = default(T?);
            try
            {
                data = await dataManager.getDataAndStoreDataFromApi<T>(uri);
            }
            catch (Exception ex)
            {
                // TODO: LOG
            }

            return data;
        }

        private T? GetDataFromFile<T>(string uri)
        {
            T? data = default(T?);
            try
            {
                data = dataManager.getDataFromStoredFile<T>(uri);
            }
            catch (Exception ex)
            {
                // TODO: LOG
            }
            return data;
        }
    }
}
