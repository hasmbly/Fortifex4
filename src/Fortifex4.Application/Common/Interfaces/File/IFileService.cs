using System.Collections.Generic;

namespace Fortifex4.Application.Common.Interfaces.File
{
    public interface IFileService
    {
        IList<T> ReadCSVFile<T>(string filePath);
        T ReadJSONFile<T>(string filePath);
    }
}