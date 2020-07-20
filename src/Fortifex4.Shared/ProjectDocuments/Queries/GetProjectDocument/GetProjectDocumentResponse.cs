using System;
using Fortifex4.Shared.Common;

namespace Fortifex4.Shared.ProjectDocuments.Queries.GetProjectDocument
{
    public class GetProjectDocumentResponse : GeneralResponse
    {
        public int ProjectDocumentID { get; set; }
        public int ProjectID { get; set; }
        public string Title { get; set; }
        public string OriginalFileName { get; set; }
        public Guid DocumentID { get; set; }
    }
}