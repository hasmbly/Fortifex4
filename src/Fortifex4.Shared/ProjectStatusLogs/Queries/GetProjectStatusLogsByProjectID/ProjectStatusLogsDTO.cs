using System;
using Fortifex4.Domain.Enums;

namespace Fortifex4.Shared.ProjectStatusLogs.Queries.GetProjectStatusLogsByProjectID
{
    public class ProjectStatusLogDTO
    {
        public DateTimeOffset LastModified { get; set; }
        public ProjectStatus ProjectStatus { get; set; }
        public string Comment { get; set; }

        public string ProjectStatusDisplayText
        {
            get
            {
                return this.ProjectStatus switch
                {
                    ProjectStatus.Draft => "Draft",
                    ProjectStatus.SubmittedForApproval => "Submitted for approval",
                    ProjectStatus.Approved => "Approved",
                    ProjectStatus.Returned => "Returned",
                    ProjectStatus.Rejected => "Rejected",
                    _ => "Unknown",
                };
            }
        }
    }
}