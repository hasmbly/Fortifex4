using System;
using Fortifex4.Domain.Common;

namespace Fortifex4.Domain.Entities
{
    public class ProjectDocument : AuditableEntity
    {
        public int ProjectDocumentID { get; set; }
        public int ProjectID { get; set; }
        public string Title { get; set; }
        public Guid DocumentID { get; set; }
        public string OriginalFileName { get; set; }

        public Project Project { get; set; }
    }
}