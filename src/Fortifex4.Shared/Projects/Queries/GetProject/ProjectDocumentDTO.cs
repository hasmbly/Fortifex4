using System;

namespace Fortifex4.Shared.Projects.Queries.GetProject
{
    public class ProjectDocumentDTO
    {
        public int ProjectDocumentID { get; set; }
        public Guid DocumentID { get; set; }
        public string Title { get; set; }
        public string OriginalFileName { get; set; }
    }
}