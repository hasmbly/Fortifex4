using System.IO;
using System.Net;
using System.Text.Json;

namespace Fortifex4.WebAPI.Common
{
    public static class WebRequestHelper
    {
        public static T Get<T>(string uri)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
            httpWebRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            httpWebRequest.Method = "GET";

            using HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using Stream streamResponse = httpWebResponse.GetResponseStream();
            using StreamReader streamReader = new StreamReader(streamResponse);
            string jsonResponse = streamReader.ReadToEnd();

            var jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<T>(jsonResponse, jsonSerializerOptions);
        }

        public static TResponse Post<TRequest, TResponse>(string uri, TRequest tRequest)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
            httpWebRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            var jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            string jsonRequest = JsonSerializer.Serialize(tRequest, jsonSerializerOptions);

            using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(jsonRequest);
            }

            using HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using Stream streamResponse = httpWebResponse.GetResponseStream();
            using StreamReader streamReader = new StreamReader(streamResponse);
            string jsonResponse = streamReader.ReadToEnd();

            return JsonSerializer.Deserialize<TResponse>(jsonResponse, jsonSerializerOptions);
        }
    }
}