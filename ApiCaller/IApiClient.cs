
namespace ApiCaller
{
    public interface IApiClient
    {
        Task<Stream> GetStreamFromUrl(string relativeUri);
    }
}