using ApiCaller;
using DataSource;

namespace PruebaTecnicaVueling.DataSource
{
    public class DataManagerXml
    {
        public const string baseAddress = "http://quiet-stone-2094.herokuapp.com/";

        private readonly string folderName = $"{Environment.CurrentDirectory}/files";
        private readonly ApiXmlClient apiProcessor;
        private readonly XmlParser xmlParser;

        public DataManagerXml(ApiXmlClient apiProcessor, XmlParser xmlParser)
        {
            this.apiProcessor = apiProcessor;
            this.xmlParser = xmlParser;
        }

        public async Task<T?> getDataAndStoreDataFromApi<T>(string uri)
        {
            T? result = default(T?);
               
            using (Stream? xmlStream = await apiProcessor.GetXmlStreamFromUrl(uri))
            {
                if (xmlStream != null)
                {
                    result = xmlParser.SerializeFromStream<T>(xmlStream);
                    storeStreamAsFile(xmlStream, uri);
                } 
            }

            return result;
        }

        private void storeStreamAsFile(Stream stream, string uri)
        {
            string filePath = getFilePath(uri);

            Directory.CreateDirectory(folderName);

            using (var fileStream = File.Create(filePath))
            {
                stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(fileStream);
            }
        }

        public T? getDataFromStoredFile<T>(string uri)
        {
            string filePath = getFilePath(uri);

            return xmlParser.SerializeObjectFromFile<T>(filePath);
        }

        public string getFilePath(string uri)
        {
            return $"{folderName}/{uri}.xml";
        }
    }
}
