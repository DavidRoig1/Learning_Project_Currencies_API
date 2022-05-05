
namespace ApiCaller
{
    public interface IApiXmlClient
    {
        Task<Stream> GetXmlStreamFromUrl(string relativeUri);
    }
}