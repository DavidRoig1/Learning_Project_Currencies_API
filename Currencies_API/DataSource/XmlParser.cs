using System.Text;
using System.Xml.Serialization;

namespace DataSource
{
    public class XmlParser : IParser
    {
        public T? SerializeObjectFromFile<T>(string path)
        {
            T? result;
            using (Stream reader = new FileStream(path, FileMode.Open))
            {
                result = SerializeFromStream<T>(reader);
            }

            return result;
        }

        public T? SerializeFromString<T>(string xmlString)
        {
            T? result;

            using (Stream reader = new MemoryStream(Encoding.UTF8.GetBytes(xmlString)))
            {
                result = SerializeFromStream<T>(reader);
            }

            return result;
        }

        public T? SerializeFromStream<T>(Stream stream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            return (T?)serializer.Deserialize(stream);
        }


    }
}
