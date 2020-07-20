using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using Fortifex4.Application.Common.Interfaces.File;
using Fortifex4.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Fortifex4.Infrastructure.File.Default
{
    public class DefaultFileService : IFileService
    {
        private static readonly IList<byte> _allowedChars = new List<byte> { };

        // For more file signatures, see the File Signatures Database (https://www.filesignatures.net/)
        // and the official specifications for the file types you wish to add.
        private static readonly Dictionary<string, List<byte[]>> _fileSignature = new Dictionary<string, List<byte[]>>
        {
            { ".pdf", new List<byte[]> { new byte[] { 0x25, 0x50, 0x44, 0x46 } } }
        };

        private IList<string> PermittedExtensions
        {
            get
            {
                List<string> permittedExtensions = new List<string>
                {
                    ".txt",
                    ".csv"
                };
                permittedExtensions.AddRange(_fileSignature.Keys);

                return permittedExtensions;
            }
        }

        private readonly ILogger<DefaultFileService> _logger;
        private readonly IConfiguration _configuration;

        public DefaultFileService(ILogger<DefaultFileService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
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

        public async Task<ProcessFileResult> ProcessFile(IFormFile formFile)
        {
            var result = new ProcessFileResult();

            var options = _configuration.GetSection(FortifexOptions.RootSection).Get<FortifexOptions>();

            var fileName = WebUtility.HtmlEncode(formFile.FileName);

            if (formFile.Length > options.FileSizeLimit)
            {
                result.ErrorMessage = $"File {fileName} is too large.";
            }
            else
            {
                try
                {
                    using var memoryStream = new MemoryStream();
                    await formFile.CopyToAsync(memoryStream);

                    if (memoryStream.Length > 0)
                    {
                        result.IsSuccessful = true;
                        result.FileContent = memoryStream.ToArray();
                    }
                    else
                    {
                        result.ErrorMessage = $"File {fileName} is empty.";
                    }
                }
                catch
                {
                    result.ErrorMessage = $"Failed to process Project Document {fileName}";
                }
            }

            return result;
        }

        public async Task<SaveFileResult> SaveFile(IList<byte> fileContent, string folderPath, string fileName)
        {
            var result = new SaveFileResult();

            try
            {
                Directory.CreateDirectory(folderPath);
                var filePath = Path.Combine(folderPath, fileName);

                using var fileStream = System.IO.File.Create(filePath);
                await fileStream.WriteAsync(fileContent.ToArray());

                result.IsSuccessful = true;
            }
            catch
            {
                result.ErrorMessage = $"Failed to upload Project Document {fileName}";
            }

            return result;
        }

        public async Task<RemoveFileResult> RemoveFile(string folderPath, string fileName)
        {
            var result = new RemoveFileResult();

            var filePath = Path.Combine(folderPath, fileName);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
                result.IsSuccessful = true;
            }

            return await Task.FromResult(result);
        }

        private static bool IsValidFileExtensionAndSignature(string fileName, Stream data, IList<string> permittedExtensions)
        {
            if (string.IsNullOrEmpty(fileName) || data == null || data.Length == 0)
            {
                return false;
            }

            var ext = Path.GetExtension(fileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
            {
                return false;
            }

            data.Position = 0;

            using var reader = new BinaryReader(data);

            if (ext.Equals(".txt") || ext.Equals(".csv"))
            {
                if (_allowedChars.Count == 0)
                {
                    // Limits characters to ASCII encoding.
                    for (var i = 0; i < data.Length; i++)
                    {
                        if (reader.ReadByte() > sbyte.MaxValue)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    // Limits characters to ASCII encoding and
                    // values of the _allowedChars array.
                    for (var i = 0; i < data.Length; i++)
                    {
                        var b = reader.ReadByte();
                        if (b > sbyte.MaxValue ||
                            !_allowedChars.Contains(b))
                        {
                            return false;
                        }
                    }
                }

                return true;
            }

            // File signature check
            // --------------------
            // With the file signatures provided in the _fileSignature
            // dictionary, the following code tests the input content's
            // file signature.
            var signatures = _fileSignature[ext];
            var headerBytes = reader.ReadBytes(signatures.Max(m => m.Length));

            return signatures.Any(signature => headerBytes.Take(signature.Length).SequenceEqual(signature));
        }
    }
}