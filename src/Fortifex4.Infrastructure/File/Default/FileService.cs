using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using CsvHelper;
using Fortifex4.Application.Common.Interfaces.File;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Fortifex4.Infrastructure.File.Default
{
    public class DefaultFileService : IFileService
    {
        private readonly ILogger<DefaultFileService> _logger;

        public DefaultFileService(ILogger<DefaultFileService> logger)
        {
            _logger = logger;
        }

        public IList<T> ReadCSVFile<T>(string filePath)
        {
            IList<T> result = new List<T>();

            using (StreamReader reader = new StreamReader(filePath))
            {
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                foreach (var item in csv.GetRecords<T>())
                {
                    result.Add(item);
                }
            }

            return result;
        }

        public T ReadJSONFile<T>(string filePath)
        {
            _logger.LogDebug(filePath);

            var jsonString = System.IO.File.ReadAllText(filePath, Encoding.UTF8);

            return JsonConvert.DeserializeObject<T>(jsonString);
        }
    }
}