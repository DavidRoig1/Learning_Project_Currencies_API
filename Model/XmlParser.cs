using System.Xml.Serialization;

namespace Model
{
    public static class XmlParser
    {
        public static T? SerializeObjectFromFile<T>(string path)
        {
            T? result = default;
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (Stream reader = new FileStream(path, FileMode.Open))
            {
                // Call the Deserialize method to restore the object's state.
                result = (T)serializer.Deserialize(reader);
            }

            return result;
        }
    }
}
