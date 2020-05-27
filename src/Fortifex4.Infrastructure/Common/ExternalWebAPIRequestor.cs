using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Fortifex4.Infrastructure.Common
{
    public static class ExternalWebAPIRequestor
    {
        private const string UserAgentForHttpWebRequest = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.77 Safari/537.36";

        public static string Get(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using Stream stream = response.GetResponseStream();
            using StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        public static async Task<string> GetAsync(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
            using Stream stream = response.GetResponseStream();
            using StreamReader reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }

        public static T Get<T>(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using Stream stream = response.GetResponseStream();
            using StreamReader reader = new StreamReader(stream);
            string jsonString = reader.ReadToEnd();

            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        public static async Task<T> GetAsync<T>(string uri, IDictionary<string, string> additionalHttpHeaders = null)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
            httpWebRequest.UserAgent = UserAgentForHttpWebRequest;
            httpWebRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            if (additionalHttpHeaders != null)
            {
                foreach (var additionalHeader in additionalHttpHeaders)
                {
                    httpWebRequest.Headers[additionalHeader.Key] = additionalHeader.Value;
                }
            }

            using HttpWebResponse httpWebResponse = (HttpWebResponse)await httpWebRequest.GetResponseAsync();
            using Stream stream = httpWebResponse.GetResponseStream();
            using StreamReader streamReader = new StreamReader(stream);
            var jsonString = await streamReader.ReadToEndAsync();

            return JsonConvert.DeserializeObject<T>(jsonString);
        }
    }
}