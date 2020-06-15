using Fortifex4.Domain.Common;
using Fortifex4.Domain.Enums;

namespace Fortifex4.Domain.Entities
{
    public class ProjectStatusLog : AuditableEntity
    {
        public int ProjectStatusLogID { get; set; }
        public int ProjectID { get; set; }
        public ProjectStatus ProjectStatus { get; set; }
        public string Comment { get; set; }

        public Project Project { get; set; }
    }
}