using System.Collections.Generic;

namespace Fortifex4.Application.Common.Interfaces.File
{
    public class ProcessFileResult
    {
        public bool IsSuccessful { get; set; }
        public string ErrorMessage { get; set; }
        public IList<byte> FileContent { get; set; }
    }
}