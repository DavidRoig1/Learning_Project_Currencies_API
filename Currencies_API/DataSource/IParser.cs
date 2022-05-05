
namespace DataSource
{
    public interface IParser
    {
        T? SerializeFromStream<T>(Stream stream);
        T? SerializeFromString<T>(string xmlString);
        T? SerializeObjectFromFile<T>(string path);
    }
}