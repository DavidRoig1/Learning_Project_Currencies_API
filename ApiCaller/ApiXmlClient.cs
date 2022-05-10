using System.Net.Http.Headers;

namespace ApiCaller
{
    public class ApiXmlClient : IApiClient
    {
        /// <summary>
        /// This client is static because it is able to support concurrent calls
        /// and is thread safe. Therefore there is no need to create multiple clients
        /// </summary>
        private static HttpClient apiClient { get; set; }

        public ApiXmlClient(string? baseAddress = null)
        {
            if (apiClient == null)
            {
                apiClient = new HttpClient();
                apiClient.DefaultRequestHeaders.Accept.Clear();
                apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                if (baseAddress != null)
                    apiClient.BaseAddress = new Uri(baseAddress);
            }
        }

        public async Task<Stream> GetStreamFromUrl(string relativeUri)
        {
            //This call is not inside a using because that would close the stream before it can be 
            //returned;
            HttpResponseMessage response = await apiClient.GetAsync(relativeUri);

            if (response.IsSuccessStatusCode)
            {
                Stream result = await response.Content.ReadAsStreamAsync();
                return result;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }

        public async Task<string> GetXmlStringFromUrl(string relativeUri)
        {
            using (HttpResponseMessage response = await apiClient.GetAsync(relativeUri))
            {
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
