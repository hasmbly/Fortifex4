using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Fortifex4.Application.Common.Interfaces.File
{
    public interface IFileService
    {
        IList<T> ReadCSVFile<T>(string filePath);
        T ReadJSONFile<T>(string filePath);
        Task<ProcessFileResult> ProcessFile(IFormFile formFile);
        Task<SaveFileResult> SaveFile(IList<byte> fileContent, string folderPath, string fileName);
        Task<RemoveFileResult> RemoveFile(string folderPath, string fileName);
    }
}