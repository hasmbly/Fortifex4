using System.Collections.Generic;

namespace Fortifex4.Shared.ProjectStatusLogs.Queries.GetProjectStatusLogsByProjectID
{
    public class GetProjectStatusLogsByProjectIDResponse
    {
        public IList<ProjectStatusLogDTO> ProjectStatusLogs { get; set; }

        public GetProjectStatusLogsByProjectIDResponse()
        {
            this.ProjectStatusLogs = new List<ProjectStatusLogDTO>();
        }
    }
}