
namespace Currencies_API.DataSource
{
    public interface IDataManager
    {
        Task<T?> getDataAndStoreDataFromApi<T>(string uri);
        T? getDataFromStoredFile<T>(string uri);
        string getFilePath(string uri);
    }
}